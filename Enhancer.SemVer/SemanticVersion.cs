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
    /// Represents a semantic version compatible with the standard proposed at https://semver.org/.
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
        /// Gets the major version node.
        /// </summary>
        public uint Major { get; }

        /// <summary>
        /// Gets the minor version node.
        /// </summary>
        public uint Minor { get; }

        /// <summary>
        /// Gets the patch version node.
        /// </summary>
        public uint Patch { get; }

        /// <summary>
        /// Gets the pre-release version node.
        /// </summary>
        public IReadOnlyList<string> PreRelease { get; }

        /// <summary>
        /// Gets the build meta-data node.
        /// </summary>
        public IReadOnlyList<string> MetaData { get; }

        /// <summary>
        /// Gets a value indicating, if the current version identifies an initial release.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the current version identifies an initial release, otherwise <see langword="false"/>.
        /// </value>
        public bool IsInitial => Major == 0;

        /// <summary>
        /// Gets a value indicating whether the current version is a pre-release.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the current version is a pre-release, otherwise <see langword="false"/>.
        /// </value>
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
            return identifier.Length > 0
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
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="version">The version string to parse</param>
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
                    case ParsingPhase.MetaData:
                        ProcessIdentifiers(c);
                        break;
                }
            }

            switch (phase)
            {
                case ParsingPhase.Major:
                case ParsingPhase.Minor:
                    throw Error();
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
                            case ParsingPhase.Patch:
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
                                      || alnum >= 'A' && alnum <= 'Z':
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
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SemanticVersion() : this(0, 0, 0, new object[0], new object[0]) { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch)
            : this(major, minor, patch, new object[0], new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch, params object[] preRelease)
            : this(major, minor, patch, preRelease, new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        [ExcludeFromCodeCoverage]
        public SemanticVersion(uint major, uint minor, uint patch, IEnumerable<object> preRelease)
            : this(major, minor, patch, preRelease, new object[0])
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> instance.
        /// </summary>
        /// <param name="major">Specifies the major node for the version.</param>
        /// <param name="minor">Specifies the minor node for the version.</param>
        /// <param name="patch">Specifies the patch node for the version.</param>
        /// <param name="preRelease">Defines the identifiers of the pre-release node for the version.</param>
        /// <param name="metadata">Defines the identifiers of the build meta-data node for the version.</param>
        [ExcludeFromCodeCoverage]
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

            preRelease = preRelease.Select(identifier => identifier ?? "");
            metadata   = metadata  .Select(identifier => identifier ?? "");

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
        public override string ToString()
        {
            return ToString("3", null);
        }

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
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format != null && format != "0" && format != "1" && format != "2" && format != "3")
            {
                throw new FormatException();
            }

            return $"{Major:#0}.{Minor:#0}.{Patch:#0}"
                 + ((format == "1" || format == "3") && PreRelease.Count != 0 ? $"-{Join(".", PreRelease)}" : "")
                 + ((format == "2" || format == "3") && MetaData.Count   != 0 ? $"+{Join(".", MetaData)}"   : "");
        }

        /// <summary>
        /// Calculates a hash code for this semantic version instance.
        /// </summary>
        /// <returns>A hash code for this version.</returns>
        [ExcludeFromCodeCoverage]
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
            return new SemanticVersion(Major, Minor, Patch, PreRelease.ToArray().Clone(), MetaData.ToArray().Clone());
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">The version to compare to.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> are equals,
        /// the <see cref="PreRelease"/> have the same amount of identifiers, and all of them are equal one another,
        /// and they appear in the same order, otherwise <see langword="false"/>.
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
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="SemanticVersion"/> instance, and
        /// <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> are equals, the <see cref="PreRelease"/>
        /// have the same amount of identifiers, and all of them are equal one another, and they appear in the same
        /// order, otherwise <see langword="false"/>.
        /// </returns>
        [ExcludeFromCodeCoverage]
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
        /// <para>If the current <see cref="Major"/> is less then <paramref name="other"/>'s <see cref="Major"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> is the same,
        /// but the current <see cref="Minor"/> is less then <paramref name="other"/>'s <see cref="Minor"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> and <see cref="Minor"/> is the same,
        /// but the current <see cref="Patch"/> is less then <paramref name="other"/>'s <see cref="Patch"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have any identifiers specified in pre-release node, while the other have not.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have more identifiers specified in pre-release node, than the other.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but one of the
        /// pre-release identifier is considered to be preceding than the other, either by numerically or lexically,
        /// or the current version specifies a number whereas the other specifies a string.
        /// </para>
        /// </term>
        /// </item>
        /// <item>
        /// <term>0</term>
        /// <term>
        /// If <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> are equal, and the pre-release nodes
        /// have exactly the same amount of identifiers and are all of them are the same and are in the same order.
        /// </term>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <term>
        /// <para>If the current <see cref="Major"/> is greater then <paramref name="other"/>'s <see cref="Major"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> is the same,
        /// but the current <see cref="Minor"/> is greater then <paramref name="other"/>'s <see cref="Minor"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> and <see cref="Minor"/> is the same,
        /// but the current <see cref="Patch"/> is greater then <paramref name="other"/>'s <see cref="Patch"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have no identifiers specified in pre-release node, while the other have.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have less identifiers specified in pre-release node, than the other.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but one of the
        /// pre-release identifier is considered to be preceding than the other, either by numerically or lexically,
        /// or the current version specifies a string whereas the other specifies a number.
        /// </para>
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This comparing function strictly follows the rules of what is specified in the https://semver.org
        /// specification's 11th point.
        /// </remarks>
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

            for (i = 0; i < Min(PreRelease.Count, other.PreRelease.Count); ++i)
            {
                if ((cmpRes = StringComparer.InvariantCulture.Compare(PreRelease[i], other.PreRelease[i])) != 0)
                    return cmpRes;
            }

            if (i < other.PreRelease.Count) return -1;
            if (i < PreRelease.Count) return 1;

            return 0;
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="obj">The other version to compare the current version with.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> isn't a <see cref="SemanticVersion"/> instance.
        /// </exception>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Condition</term>
        /// </listheader>
        /// <item>
        /// <term>-1</term>
        /// <term>
        /// <para>If the current <see cref="Major"/> is less then <paramref name="obj"/>'s <see cref="Major"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> is the same,
        /// but the current <see cref="Minor"/> is less then <paramref name="obj"/>'s <see cref="Minor"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> and <see cref="Minor"/> is the same,
        /// but the current <see cref="Patch"/> is less then <paramref name="obj"/>'s <see cref="Patch"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have any identifiers specified in pre-release node, while the other have not.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have more identifiers specified in pre-release node, than the other.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but one of the
        /// pre-release identifier is considered to be preceding than the other, either by numerically or lexically,
        /// or the current version specifies a number whereas the other specifies a string.
        /// </para>
        /// </term>
        /// </item>
        /// <item>
        /// <term>0</term>
        /// <term>
        /// If <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> are equal, and the pre-release nodes
        /// have exactly the same amount of identifiers and are all of them are the same and are in the same order.
        /// </term>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <term>
        /// <para>If the current <see cref="Major"/> is greater then <paramref name="obj"/>'s <see cref="Major"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> is the same,
        /// but the current <see cref="Minor"/> is greater then <paramref name="obj"/>'s <see cref="Minor"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>If the <see cref="Major"/> and <see cref="Minor"/> is the same,
        /// but the current <see cref="Patch"/> is greater then <paramref name="obj"/>'s <see cref="Patch"/>,</para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have no identifiers specified in pre-release node, while the other have.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but the current
        /// instance have less identifiers specified in pre-release node, than the other.
        /// </para>
        /// <para><em>- OR -</em></para>
        /// <para>
        /// If the <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/> is the same, but one of the
        /// pre-release identifier is considered to be preceding than the other, either by numerically or lexically,
        /// or the current version specifies a string whereas the other specifies a number.
        /// </para>
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This comparing function strictly follows the rules of what is specified in the https://semver.org
        /// specification's 11th point.
        /// </remarks>
        [ExcludeFromCodeCoverage]
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
        /// Compares the current version with the specified version, and determines if the versions
        /// are only have backward-compatible.
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
        /// Compares two semantic version whether the first version successes the second.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> successes <paramref name="b"/>, otherwise <see langword="false"/>.
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
        /// Compares two semantic version whether the first version successes the second, or are they the same.
        /// </summary>
        /// <param name="a">The first version to compare.</param>
        /// <param name="b">The second version to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="a"/> successes <paramref name="b"/> or are they the same,
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

        /// <summary>
        /// Converts the specified semantic version to a <see cref="Version"/>.
        /// </summary>
        /// <param name="version">The version to convert.</param>
        /// <remarks>
        /// The converted version will only have it's major and minor node set.
        /// The <see cref="Patch"/>, <see cref="PreRelease"/> and <see cref="MetaData"/> values will not be transferred
        /// to the result by any means.
        /// </remarks>
        public static explicit operator Version(SemanticVersion version)
        {
            return new Version((int)version.Major, (int)version.Minor);
        }

        /// <summary>
        /// Converts the specified <see cref="Version"/> to a <see cref="SemanticVersion"/>.
        /// </summary>
        /// <param name="version">The version to convert.</param>
        /// <remarks>
        /// The converted version will only have it's major and minor node set.
        /// The <see cref="Patch"/> node will be zero.
        /// </remarks>
        public static explicit operator SemanticVersion(Version version)
        {
            return new SemanticVersion((uint)version.Major, (uint)version.Minor, 0);
        }
    }
}
