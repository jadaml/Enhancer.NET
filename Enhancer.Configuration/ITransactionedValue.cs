/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Configuration.
 *
 * Enhancer.Configuration is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Configuration is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Configuration.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a setting object who's value changes can be finalized or reverted.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public interface ITransactionedValue : IValue
    {
        /// <summary>
        /// Gets the finalized value.
        /// </summary>
        /// <value>
        /// The finalized value when loaded or after a <see cref="Commit()"/>.
        /// </value>
        object Reference { get; }

        /// <summary>
        /// Gets a value indicating whether the value was modified.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the value got modified since last saved or
        /// first loaded, otherwise <see langword="false"/>.
        /// </value>
        bool IsModified { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the value changes should be
        /// committed automatically or not.
        /// </summary>
        /// <value>
        /// When <see langword="true"/> all changes made to
        /// <see cref="IValue.Value"/> will be committed immediately.
        /// When <see langword="false"/> the <see cref="Commit()"/> method must
        /// be called to finalize the changes.
        /// </value>
        bool AutoCommit { get; set; }

        /// <summary>
        /// Raised when the changes were finalized.
        /// </summary>
        event EventHandler Committed;

        /// <summary>
        /// Raised when the changes were reverted to the <see cref="Reference"/>
        /// value.
        /// </summary>
        event EventHandler RolledBack;

        /// <summary>
        /// Finalizes the changes made to the value.
        /// </summary>
        void Commit();

        /// <summary>
        /// Reverts the changes made to the value to the <see cref="Reference"/>
        /// value.
        /// </summary>
        void Rollback();
    }

    /// <summary>
    /// Represents a setting object who's value changes can be finalized or reverted.
    /// </summary>
    /// <typeparam name="T">The base type of the value.</typeparam>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public interface ITransactionedValue<T> : IValue<T>
    {
        /// <summary>
        /// Gets the finalized value.
        /// </summary>
        /// <value>
        /// The finalized value when loaded or after a <see cref="Commit()"/>.
        /// </value>
        T Reference { get; }

        /// <summary>
        /// Gets a value indicating whether the value was modified.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the value got modified since last saved or
        /// first loaded, otherwise <see langword="false"/>.
        /// </value>
        bool IsModified { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the value changes should be
        /// committed automatically or not.
        /// </summary>
        /// <value>
        /// When <see langword="true"/> all changes made to
        /// <see cref="IValue{T}.Value"/> will be committed immediately.
        /// When <see langword="false"/> the <see cref="Commit()"/> method must
        /// be called to finalize the changes.
        /// </value>
        bool AutoCommit { get; set; }

        /// <summary>
        /// Raised when the changes were finalized.
        /// </summary>
        event EventHandler Committed;

        /// <summary>
        /// Raised when the changes were reverted to the <see cref="Reference"/>
        /// value.
        /// </summary>
        event EventHandler RolledBack;

        /// <summary>
        /// Finalizes the changes made to the value.
        /// </summary>
        void Commit();

        /// <summary>
        /// Reverts the changes made to the value to the <see cref="Reference"/>
        /// value.
        /// </summary>
        void Rollback();
    }
}
