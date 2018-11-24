using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegexChecker
{
	public class Model : INotifyPropertyChanged
	{
		static readonly ReadOnlyCollection<MatchResult> EmptyMatches = new ReadOnlyCollection<MatchResult>(new MatchResult[0]);

		string _pattern = string.Empty;
		public string Pattern
		{
			get => _pattern;
			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				Utils.SetProperty(ref _pattern, value, PropertyChanged, this);
			}
		}

		RegexOptions _options;
		public RegexOptions Options
		{
			get => _options;
			set => Utils.SetProperty(ref _options, value, PropertyChanged, this);
		}

		string _target = string.Empty;
		public string Target
		{
			get => _target;
			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				Utils.SetProperty(ref _target, value, PropertyChanged, this);
			}
		}

		string _replacement = string.Empty;
		public string Replacement
		{
			get => _replacement;
			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				Utils.SetProperty(ref _replacement, value, PropertyChanged, this);
			}
		}

		ReadOnlyCollection<MatchResult> _matches;
		public ReadOnlyCollection<MatchResult> Matches
		{
			get => _matches;
			private set => Utils.SetProperty(ref _matches, value, PropertyChanged, this);
		}

		public void RunRegex()
		{
			try
			{
				var regex = new Regex(Pattern, Options);
				Matches = new ReadOnlyCollection<MatchResult>(regex.Matches(Target).Cast<Match>().Select((x, i) => new MatchResult(x, i, Replacement)).ToArray());
			}
			catch (ArgumentException) { Matches = EmptyMatches; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class MatchResult
	{
		public MatchResult(Match match, int number, string replacement)
		{
			Number = number;
			Success = match.Success;
			Index = match.Index;
			Length = match.Length;
			Value = match.Value;
			Replaced = match.Result(replacement);
			Groups = new ReadOnlyCollection<GroupWrapper>(match.Groups.Cast<Group>().Select((x, i) => (Item: x, Index: i)).Where(x => x.Index > 0).Select(x => new GroupWrapper(x.Item, x.Index)).ToArray());
		}
		public int Number { get; }
		public bool Success { get; }
		public int Index { get; }
		public int Length { get; }
		public string Value { get; }
		public string Replaced { get; }
		public ReadOnlyCollection<GroupWrapper> Groups { get; }
	}

	public class GroupWrapper
	{
		public GroupWrapper(Group group, int number)
		{
			Name = group.Name;
			Number = number;
			Success = group.Success;
			Index = group.Index;
			Length = group.Length;
			Value = group.Value;
			Captures = new ReadOnlyCollection<CaptureWrapper>(group.Captures.Cast<Capture>().Select((x, i) => new CaptureWrapper(x, i)).ToArray());
		}
		public string Name { get; }
		public int Number { get; }
		public bool Success { get; }
		public int Index { get; }
		public int Length { get; }
		public string Value { get; }
		public ReadOnlyCollection<CaptureWrapper> Captures { get; }
	}

	public class CaptureWrapper
	{
		public CaptureWrapper(Capture capture, int number)
		{
			Number = number;
			Index = capture.Index;
			Length = capture.Length;
			Value = capture.Value;
		}
		public int Number { get; }
		public int Index { get; }
		public int Length { get; }
		public string Value { get; }
	}
}
