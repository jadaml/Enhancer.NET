/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.SemVer.
 *
 * Enhancer.SemVer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.SemVer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.SemVer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using static System.Math;
using static System.String;

namespace Enhancer.Assemblies
{
    /// <summary>
    /// Represents a semantic version compatible with the standard proposed at
    /// <see href="https://semver.org/" target="_blank"/>.
    /// </summary>
    public class SemanticVersion : ICloneable, IEquatable<SemanticVersion>, IComparable, IComparable<SemanticVersion>, IFormattable
    {
        private enum ParsingPhase
        {
            Major,
            Minor,
            Patch,
            PreRelease,
            MetaData,
        }

        /// <summary>
        /// Represents an empty semantic version, with the lowest possible version number, that is not a pre-release version.
        /// </summary>
        public static readonly SemanticVersion Empty = new SemanticVersion();

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        public uint Major { get; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        public uint Minor { get; }

        /// <summary>
        /// Gets the patch version number.
        /// </summary>
        public uint Patch { get; }

        /// <summary>
        /// Gets the pre-release version node.
        /// </summary>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public IReadOnlyList<string> PreRelease { get; }

        /// <summary>
        /// Gets the build meta-data node.
        /// </summary>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public IReadOnlyList<string> MetaData { get; }

        /// <summary>
        /// Gets a value indicating, if the current version identifies an
        /// initial development release.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the current version identifies an initial
        /// development release, otherwise <see langword="false"/>.
        /// </value>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public bool IsDevelopmentVersion => Major == 0;

        /// <summary>
        /// Gets a value indicating whether the current version is a pre-release.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the current version is a pre-release, otherwise <see langword="false"/>.
        /// </value>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public bool IsPreRelease => PreRelease.Count > 0;

        /// <summary>
        /// Checks if an identifier is valid or not.
        /// </summary>
        /// <param name="identifier">The identifier to test.</param>
        /// <returns><see langword="true"/> if the identifier is valid, otherwise <see langword="false"/>.</returns>
        /// <remarks>
        /// An identifier is what is separated by periods, and found in either the pre-release node, or the meta-data node.
        /// </remarks>
        public static bool ValidIdentifier(string identifier)
        {
            return identifier != null && identifier.Length > 0
            // only digits and no leading zero.
                && (identifier.All(c => c >= '0' && c <= '9') && (identifier.Length == 1 || identifier.First() != '0')
            // Have other than digits but only alphanumeric, and hyphen.
                 || identifier.Any(c => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '-')
                  && identifier.All(c => c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '-'));
        }
        
        /// <summary>
        /// Parses the given version string in to a <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="version">The version string to parse.</param>
        /// <returns>The parsed instance of <see cref="SemanticVersion"/>.</returns>
        /// <exception cref="FormatException">If failed to parse the provided string.</exception>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public static SemanticVersion Parse(string version)
        {
            try
            {
                return new SemanticVersion(version);
            }
            catch (ArgumentException ex)
            {
                throw new FormatException("The format of the provided string is not a valid version string.", ex);
            }
        }

        /// <summary>
        /// Makes an attempt to parse the given version string in to a <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="input">The version string to parse.</param>
        /// <param name="version">The resulting <see cref="SemanticVersion"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the version string was successfully parsed, and <paramref name="version"/>
        /// holds an instance representing the parsed string, or <see langword="false"/> if failed to parse the version
        /// string, in which case the resulting <paramref name="version"/> has nothing to do with the version string.
        /// </returns>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public static bool TryParse(string input, out SemanticVersion version)
        {
            try
            {
                version = Parse(input);
                return true;
            }
            catch (FormatException)
            {
                version = Empty;
                return false;
            }
        }

        /// <summary>
        /// Helper method, to convert the provided object to a string, using the invariant culture.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The string representing the object.</returns>
        private static string ObjectAsString(object obj)
        {
            return (obj as IFormattable)?.ToString(null, CultureInfo.InvariantCulture) ?? obj.ToString();
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance by parsing the
        /// provided string as a semantic version.
        /// </summary>
        /// <param name="version">The version string to parse</param>
        /// <exception cref="NullReferenceException">
        /// If <paramref name="version"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="version"/> is an empty string.
        /// </exception>
        /// <exception cref="FormatException">
        /// If failed to parse the provided string.
        /// </exception>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public SemanticVersion(string version) : this()
        {
            if (version is null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (IsNullOrEmpty(version))
            {
                throw new ArgumentException("The version string cannot be null or empty.", nameof(version));
            }

            string       acc         = "";
            ParsingPhase phase       = 0;
            List<string> identifiers = new List<string>();
            uint?        vernodeResult;

            foreach (char c in version)
            {
                switch (phase)
                {
                    case ParsingPhase.Major:
                        vernodeResult = ProcessVersionNode(c);
                        if (vernodeResult.HasValue) Major = vernodeResult.Value;
                        break;
                    case ParsingPhase.Minor:
                        vernodeResult = ProcessVersionNode(c);
                        if (vernodeResult.HasValue) Minor = vernodeResult.Value;
                        break;
                    case ParsingPhase.Patch:
                        vernodeResult = ProcessVersionNode(c);
                        if (vernodeResult.HasValue) Patch = vernodeResult.Value;
                        break;
                    case ParsingPhase.PreRelease:
                        if (ProcessIdentifiers(c))
                        {
                            PreRelease = identifiers.ToArray();
                            identifiers.Clear();
                        }
                        break;
                    default: // ParsingPhase.MetaData
                        ProcessIdentifiers(c);
                        break;
                }
            }

            switch (phase)
            {
                case ParsingPhase.Patch:
                    Patch = ProcessVersionNode(null).Value;
                    break;
                case ParsingPhase.PreRelease:
                    ProcessIdentifiers(null);
                    PreRelease = identifiers.ToArray();
                    break;
                case ParsingPhase.MetaData:
                    ProcessIdentifiers(null);
                    MetaData = identifiers.ToArray();
                    break;
                default:
                    throw Error();
            }

            uint? ProcessVersionNode(char? c)
            {
                uint? result = null;
                switch (c)
                {
                    case char digit when digit >= '0' && digit <= '9':
                        acc += digit;
                        break;
                    case null:
                        GatherResult();
                        break;
                    case char terminate when terminate == '.':
                        GatherResult();
                        switch (phase)
                        {
                            case ParsingPhase.Major:
                                phase = ParsingPhase.Minor;
                                break;
                            case ParsingPhase.Minor:
                                phase = ParsingPhase.Patch;
                                break;
                            default:
                                throw Error();
                        }
                        break;
                    case char terminate when terminate == '-':
                        if (phase != ParsingPhase.Patch) throw Error();
                        GatherResult();
                        phase = ParsingPhase.PreRelease;
                        break;
                    case char terminate when terminate == '+':
                        if (phase != ParsingPhase.Patch) throw Error();
                        GatherResult();
                        phase = ParsingPhase.MetaData;
                        break;
                    default:
                        throw Error();
                }
                return result;

                void GatherResult()
                {
                    if (acc.Length > 1 && acc.First() == '0') throw Error();
                    result = uint.Parse(acc, NumberStyles.None);
                    acc = "";
                }
            }

            bool ProcessIdentifiers(char? c)
            {
                switch (c)
                {
                    case null:
                        GatherIdentifier();
                        return true;
                    case char alnum when alnum >= '0' && alnum <= '9'
                                      || alnum >= 'a' && alnum <= 'z'
                                      || alnum >= 'A' && alnum <= 'Z'
                                      || alnum == '-':
                        acc += alnum;
                        break;
                    case char terminate when terminate == '.':
                        GatherIdentifier();
                        break;
                    case char terminate when terminate == '+':
                        if (phase == ParsingPhase.MetaData) throw Error();
                        GatherIdentifier();
                        phase = ParsingPhase.MetaData;
                        return true;
                    default:
                        throw Error();
                }

                return false;

                void GatherIdentifier()
                {
                    if (!ValidIdentifier(acc)) throw Error();
                    identifiers.Add(acc);
                    acc = "";
                }
            }

            Exception Error()
            {
                return new ArgumentException($"Malformed version string while parsing node: {phase}", nameof(version));
            }
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance representing
        /// a 0.0.0 initial release with no pre-release and meta data nodes.
        /// </summary>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        [ExcludeFromCodeCoverage]
        public SemanticVersion() : this(0, 0, 0, new object[0], new object[0]) { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance representing a
        /// <paramref name="major"/>.<paramref name="minor"/>.<paramref name="patch"/>
        /// semantic version with no pre-release or meta-data nodes.
        /// </summary>
        /// <param name="major">Specifies the major version number for the version.</param>
        /// <param name="minor">Specifies the minor version number for the version.</param>
        /// <param name="patch">Specifies the patch version number for the version.</param>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch)
            : this(major, minor, patch, new object[0], new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance specifying a
        /// <paramref name="major"/>.<paramref name="minor"/>.<paramref name="patch"/>
        /// semantic version where the pre-release node is constructed by the
        /// string representation of the <paramref name="preRelease"/> list
        /// of identifiers and have no meta-data node.
        /// </summary>
        /// <param name="major">Specifies the major version number for the version.</param>
        /// <param name="minor">Specifies the minor version number for the version.</param>
        /// <param name="patch">Specifies the patch version number for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="preRelease"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="preRelease"/>
        /// contains an object, who's <see cref="object.ToString()"/> method
        /// returns a string that is not a valid identified.
        /// Valid identifiers can be checked with the
        /// <see cref="ValidIdentifier(string)"/> method.
        /// </exception>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch, params object[] preRelease)
            : this(major, minor, patch, preRelease, new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance specifying a
        /// <paramref name="major"/>.<paramref name="minor"/>.<paramref name="patch"/>
        /// semantic version where the pre-release node is constructed by the
        /// string representation of the <paramref name="preRelease"/> list
        /// of identifiers and have no meta-data node.
        /// </summary>
        /// <param name="major">Specifies the major version number for the version.</param>
        /// <param name="minor">Specifies the minor version number for the version.</param>
        /// <param name="patch">Specifies the patch version number for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="preRelease"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="preRelease"/>
        /// contains an object, who's <see cref="object.ToString()"/> method
        /// returns a string that is not a valid identified.
        /// Valid identifiers can be checked with the
        /// <see cref="ValidIdentifier(string)"/> method.
        /// </exception>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch, IEnumerable<object> preRelease)
            : this(major, minor, patch, preRelease, new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance specifying a
        /// <paramref name="major"/>.<paramref name="minor"/>.<paramref name="patch"/>
        /// semantic version where the pre-release and meta-data nodes are
        /// constructed by the string representation of the
        /// <paramref name="preRelease"/> and <paramref name="metadata"/> lists
        /// of identifiers respectively.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        /// <param name="metadata">Defines the identifiers of the build meta-data node for the version.</param>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="preRelease"/> or
        /// <paramref name="metadata"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If either <paramref name="preRelease"/> or <paramref name="metadata"/>
        /// contains an object, who's <see cref="object.ToString()"/> method
        /// returns a string that is not a valid identified.
        /// Valid identifiers can be checked with the
        /// <see cref="ValidIdentifier(string)"/> method.
        /// </exception>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public SemanticVersion(uint major, uint minor, uint patch, IEnumerable<object> preRelease, IEnumerable<object> metadata)
            : this(major, minor, patch,
                  (preRelease ?? throw new ArgumentNullException(nameof(preRelease))).Select(ObjectAsString),
                  (metadata ?? throw new ArgumentNullException(nameof(metadata))).Select(ObjectAsString))
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        /// <param name="metadata">Defines the identifiers of the build meta-data node for the version.</param>
        /// <exception cref="ArgumentException">
        /// If either <paramref name="preRelease"/> or <paramref name="metadata"/>
        /// contains an object, who's <see cref="object.ToString()"/> method
        /// returns a string that is not a valid identified.
        /// </exception>
        /// <seealso cref="ValidIdentifier(string)"/>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        private SemanticVersion(uint major, uint minor, uint patch, IEnumerable<string> preRelease, IEnumerable<string> metadata)
        {
            if (!preRelease.All(ValidIdentifier))
            {
                throw new ArgumentException("Pre-release identifiers can only contain ASCII alphanumeric characters, and hyphens.", nameof(preRelease));
            }

            if (!metadata.All(ValidIdentifier))
            {
                throw new ArgumentException("Meta-data identifiers can only contain ASCII alphanumeric characters, and hyphens.", nameof(metadata));
            }

            preRelease = preRelease.Where(identifier => !IsNullOrEmpty(identifier));
            metadata   = metadata  .Where(identifier => !IsNullOrEmpty(identifier));

            Major      = major;
            Minor      = minor;
            Patch      = patch;
            PreRelease = preRelease.ToArray();
            MetaData   = metadata  .ToArray();
        }

        /// <summary>
        /// Returns the string representation of the current instance.
        /// </summary>
        /// <returns>The string representing the semantic version instance.</returns>
        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Returns the string representation of the current instance.
        /// </summary>
        /// <param name="format">
        /// Can be either <see langword="null"/> to specify the default behavior or one of the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Results</term>
        /// </listheader>
        /// <item>
        /// <term><code>"0"</code></term>
        /// <term>
        /// Only returns the "<em>major</em>.<em>minor</em>.<em>patch</em>" portion, completely omitting the pre-release
        /// and meta-data nodes.
        /// </term>
        /// </item>
        /// <item>
        /// <term><code>"1"</code></term>
        /// <term>Includes the pre-release node, but omits the build meta-data node.</term>
        /// </item>
        /// <item>
        /// <term><code>"2"</code></term>
        /// <term>Includes the build meta-data node, but omits the pre-release node.</term>
        /// </item>
        /// <item>
        /// <term><code>"3"</code> <em>(Default)</em></term>
        /// <term>Produces the full version including the pre-release node and build meta-data.</term>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>The string representing the semantic version instance.</returns>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        [ExcludeFromCodeCoverage]
        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the current instance.
        /// </summary>
        /// <param name="format">
        /// Can be either <see langword="null"/> to specify the default behavior or one of the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Results</term>
        /// </listheader>
        /// <item>
        /// <term><code>"0"</code></term>
        /// <term>
        /// Only returns the "<em>major</em>.<em>minor</em>.<em>patch</em>" portion, completely omitting the pre-release
        /// and meta-data nodes.
        /// </term>
        /// </item>
        /// <item>
        /// <term><code>"1"</code></term>
        /// <term>Includes the pre-release node, but omits the build meta-data node.</term>
        /// </item>
        /// <item>
        /// <term><code>"2"</code></term>
        /// <term>Includes the build meta-data node, but omits the pre-release node.</term>
        /// </item>
        /// <item>
        /// <term><code>"3"</code> <em>(Default)</em></term>
        /// <term>Produces the full version including the pre-release node and build meta-data.</term>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="formatProvider">This parameter does not affects the outcome of the string result.</param>
        /// <returns>The string representing the semantic version instance.</returns>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format != null && format != "0" && format != "1" && format != "2" && format != "3")
            {
                throw new FormatException();
            }

            format = format ?? "3";

            return $"{Major:#0}.{Minor:#0}.{Patch:#0}"
                 + ((format == "1" || format == "3") && PreRelease.Count != 0 ? $"-{Join(".", PreRelease)}" : "")
                 + ((format == "2" || format == "3") && MetaData.Count   != 0 ? $"+{Join(".", MetaData)}"   : "");
        }

        /// <summary>
        /// Calculates a hash code for this semantic version instance.
        /// </summary>
        /// <returns>A hash code for this version.</returns>
        public override int GetHashCode()
        {
            return (int)(Major
                  ^ (Minor >> 2)
                  ^ (Patch >> 4)
                  ^ (PreRelease.Reverse().Aggregate(0u, (a, s) => (a >> 1) ^ (uint)s.GetHashCode()) >> 8));
        }

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>The copy of the current instance.</returns>
        public object Clone()
        {
            // Calling clone since these are arrays anyway, hence they do not really copy anything.
            return new SemanticVersion(Major, Minor, Patch, (object[])PreRelease.ToArray().Clone(), (object[])MetaData.ToArray().Clone());
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">The version to compare to.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> is not
        /// <see langword="null"/>, and
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal, and the <see cref="PreRelease"/> node have the same
        /// amount of identifiers, and all of the identifiers are the same,
        /// and they appear in the same order,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(SemanticVersion other)
        {
            return !(other is null)
                && Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && PreRelease.SequenceEqual(other.PreRelease, StringComparer.InvariantCulture);
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="obj">The version to compare to.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a
        /// <see cref="SemanticVersion"/> instance, and
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal, and the <see cref="PreRelease"/> node have the same
        /// amount of identifiers, and all of the identifiers are the same,
        /// and they appear in the same order,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SemanticVersion);
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">The other version to compare the current version with.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Condition</term>
        /// </listheader>
        /// <item>
        /// <term>-1</term>
        /// <term>
        /// <para>
        /// If the current <see cref="Major"/> version number is less than
        /// <paramref name="other"/>'s <see cref="Major"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the <see cref="Major"/> version numbers
        /// are equal,
        /// but the current <see cref="Minor"/> version number is less than
        /// <paramref name="other"/>'s <see cref="Minor"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/> and
        /// <see cref="Minor"/> version numbers
        /// are equal,
        /// but the current <see cref="Patch"/> version number is less than
        /// <paramref name="other"/>'s <see cref="Patch"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// but the current instance have identifiers specified in it's
        /// <see cref="PreRelease"/> node, while the <paramref name="other"/>
        /// have not.
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// but the current instance have less identifiers specified in
        /// <see cref="PreRelease"/> node, than the <paramref name="other"/>.
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// and the number of identifiers in <see cref="PreRelease"/> nodes
        /// are equal,
        /// but one of the <see cref="PreRelease"/> identifier
        /// at the smallest common index
        /// at which they are not equal
        /// precedes <paramref name="other"/>'s,
        /// either by numerically or lexically,
        /// or the current version identifier specifies a number
        /// whereas the <paramref name="other"/>'s specifies a string.
        /// </para>
        /// </term>
        /// </item>
        /// <item>
        /// <term>0</term>
        /// <term>
        /// If
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// and the <see cref="PreRelease"/> nodes have the same amount of
        /// identifiers
        /// and all of them are the same and are in the same order.
        /// </term>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <term>
        /// <para>
        /// If the current <see cref="Major"/> version number is greater than
        /// <paramref name="other"/>'s <see cref="Major"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/> version numbers
        /// are equal,
        /// but the current <see cref="Minor"/> version number is greater than
        /// <paramref name="other"/>'s <see cref="Minor"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/> and
        /// <see cref="Minor"/> version numbers
        /// are equal,
        /// but the current <see cref="Patch"/> version number is greater than
        /// <paramref name="other"/>'s <see cref="Patch"/> version number,
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// but the current instance have no identifiers specified in its
        /// <see cref="PreRelease"/> node, while the <paramref name="other"/>
        /// have.
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// but the current instance have more identifiers specified in its
        /// <see cref="PreRelease"/> node, than the <paramref name="other"/>.
        /// </para>
        /// <para><em>‒ OR ‒</em></para>
        /// <para>
        /// If the
        /// <see cref="Major"/>,
        /// <see cref="Minor"/> and
        /// <see cref="Patch"/> version numbers
        /// are equal,
        /// but one of the <see cref="PreRelease"/> identifier
        /// at the smallest common index
        /// at which they are not equal
        /// follows <paramref name="other"/>'s,
        /// either by numerically or lexically,
        /// or the current version identifier specifies a string
        /// whereas the <paramref name="other"/>'s specifies a number.
        /// </para>
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This comparing function strictly follows the rules of what is
        /// specified in the
        /// <see href="https://semver.org/spec/v2.0.0.html#spec-item-11">
        /// Semantic Version 2.0.0 specification's 11th point.
        /// </see>
        /// </remarks>
        /// <seealso href="https://semver.org/spec/v2.0.0.html">Semantic Version 2.0.0</seealso>
        public int CompareTo(SemanticVersion other)
        {
            int cmpRes;

            if ((cmpRes = Major.CompareTo(other.Major)) != 0) return cmpRes;
            if ((cmpRes = Minor.CompareTo(other.Minor)) != 0) return cmpRes;
            if ((cmpRes = Patch.CompareTo(other.Patch)) != 0) return cmpRes;

            if (PreRelease.Count == 0 && other.PreRelease.Count == 0) return 0;

            if (other.PreRelease.Count == 0) return -1;

            if (PreRelease.Count == 0) return 1;

            int i;
            int prmin = Min(PreRelease.Count, other.PreRelease.Count);

            for (i = 0; i < prmin; ++i)
            {
                string ida = PreRelease[i];
                string idb = other.PreRelease[i];

                bool aisnum = ida.All(char.IsDigit) && (ida.Length <= 1 || !ida.StartsWith("0"));
                bool bisnum = idb.All(char.IsDigit) && (idb.Length <= 1 || !idb.StartsWith("0"));

                if ((cmpRes = Convert.ToInt32(bisnum) - Convert.ToInt32(aisnum)) != 0)
                {
                    return cmpRes;
                }

                if (aisnum && (cmpRes = int.Parse(ida) - int.Parse(idb)) != 0
                 || (cmpRes = StringComparer.InvariantCulture.Compare(ida, idb)) != 0)
                {
                    return cmpRes;
                }
            }

            if ((cmpRes = PreRelease.Count - other.PreRelease.Count) != 0)
            {
                return cmpRes;
            }

            return 0;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj as SemanticVersion ?? throw new ArgumentException("The parameter is not the expected type.", nameof(obj)));
        }

        /// <summary>
        /// Compares the current version with the specified version, and determines if the versions
        /// are only have bug-fix changes.
        /// </summary>
        /// <param name="version">The later version to compare with.</param>
        /// <returns>
        /// <see langword="true"/> if the specified version compared to the current version only specifies minor bug-fixes or
        /// other changes, otherwise <see langword="false"/>.
        /// </returns>
        public bool IsPatch(SemanticVersion version)
        {
            return Major == version.Major && Minor == version.Minor;
        }

        /// <summary>
        /// Compares the current version with the specified version, and
        /// determines if the versions are only have backward-compatible
        /// changes.
        /// </summary>
        /// <param name="version">The later version to compare with.</param>
        /// <returns>
        /// <see langword="true"/> if the specified version compared to the current version only specifies
        /// backward-compatible changes, otherwise <see langword="false"/>.
        /// </returns>
        public bool IsBackwardCompatible(SemanticVersion version)
        {
            return Major == version.Major;
        }

        /// <summary>
        /// Compares the current version with the specified version, and determines if the versions
        /// are possibly not backward-compatible.
        /// </summary>
        /// <param name="version">The later version to compare with.</param>
        /// <returns>
        /// <see langword="true"/> if the specified version compared to the current version isn't backward-compatible,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool IsBreaking(SemanticVersion version)
        {
            return Major != version.Major;
        }

        /// <summary>
        /// Compares if two semantic versions are equal.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> and <paramref name="b"/> are considered equal, otherwise
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(SemanticVersion a, SemanticVersion b) => a?.Equals(b) ?? b is null;

        /// <summary>
        /// Compares if two semantic versions are not equal.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> and <paramref name="b"/> are considered unequal, otherwise
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(SemanticVersion a, SemanticVersion b) => !a?.Equals(b) ?? !(b is null);

        /// <summary>
        /// Compares two semantic versions whether the first version precedes the second.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> precedes <paramref name="b"/>, otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <(SemanticVersion a, SemanticVersion b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.CompareTo(b) < 0;
        }

        /// <summary>
        /// Compares two semantic version whether the first version follows the second.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> follows <paramref name="b"/>, otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >(SemanticVersion a, SemanticVersion b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.CompareTo(b) > 0;
        }

        /// <summary>
        /// Compares two semantic versions whether the first version precedes the second, or are they the same.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if either <paramref name="a"/> precedes <paramref name="b"/> or are they the same,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <=(SemanticVersion a, SemanticVersion b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.CompareTo(b) <= 0;
        }

        /// <summary>
        /// Compares two semantic version whether the first version follows the second, or are they the same.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> follows <paramref name="b"/> or are they the same,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >=(SemanticVersion a, SemanticVersion b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.CompareTo(b) >= 0;
        }
    }
}
