﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine
{
    public class EngineTickInput
    {
        public readonly IReadOnlyList<Command> Commands;

        public EngineTickInput(IEnumerable<Command> commands)
        {
            Commands = commands.ToList();
        }

        public EngineTickInput()
        {
            Commands = new Command[0];
        }
    }

    public class EngineTickResult
    {
        public readonly IEnumerable<Event> Events;

        public EngineTickResult(IEnumerable<Event> events)
        {
            Events = events;
        }
    }

    /// <summary>
    /// The single entry point from outside code into the engine.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Read-only access to the collection of entities in the engine.
        /// During a tick the contents of this collection should never change, nor the visible data
        /// within entities. However, once <seealso cref="Synchronize" /> is called all entities added
        /// during the tick will be added to this list and new data in entity components visible.
        /// </summary>
        IReadOnlyCollection<IReadOnlyEntity> Entities { get; }

        /// <summary>
        /// Advance the simulation by one tick with the given <paramref name="input" />.
        /// </summary>
        /// <param name="input">An <see cref="EngineTickInput" /> object containing commands to execute during the tick.</param>
        /// <returns>An <see cref="EngineTickResult" /> object containing events from the tick.</returns>
        /// <exception cref="InvalidOperationException">
        /// If <see cref="Synchronize" /> has not been called since the last tick
        /// ended.
        /// </exception>
        Task<EngineTickResult> Tick(EngineTickInput input);

        /// <summary>
        /// Call from the main thread once a Tick has completed to "Advance" all the entities to their next state.
        /// New entities will be added to <seealso cref="Entities" />.
        /// This ensures that public entity state is not changed until the tick is completed and the client code is ready.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the tick has not completed, or no syncronization is waiting.</exception>
        void Synchronize();
    }
}
