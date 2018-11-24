using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Livet;
using Livet.EventListeners;

namespace RegexChecker
{
	public class MainWindowViewModel : ViewModel
	{
		public MainWindowViewModel() { _model = new Model(); }

		public void Initialize()
		{
			CompositeDisposable.Add(new PropertyChangedEventListener(_model, (s, ev) =>
			{
				switch (ev.PropertyName)
				{
					case nameof(_model.Pattern):
						RaisePropertyChanged(nameof(Pattern));
						break;
					case nameof(_model.Target):
						RaisePropertyChanged(nameof(Target));
						break;
					case nameof(_model.Replacement):
						RaisePropertyChanged(nameof(Replacement));
						break;
					case nameof(_model.Matches):
						RaisePropertyChanged(nameof(Matches));
						break;
				}
			}));
		}

		readonly Model _model;

		public string Pattern
		{
			get => _model.Pattern;
			set => _model.Pattern = value;
		}

		System.Collections.IList _options;
		public System.Collections.IList Options
		{
			get => _options;
			set => Utils.SetProperty(ref _options, value, RaisePropertyChanged);
		}

		public string Target
		{
			get => _model.Target;
			set => _model.Target = value;
		}

		public string Replacement
		{
			get => _model.Replacement;
			set => _model.Replacement = value;
		}

		public ReadOnlyCollection<MatchResult> Matches => _model.Matches;

		public void RunRegex()
		{
			_model.Options = _options == null ? RegexOptions.None : _options.Cast<RegexOptions>().Aggregate(RegexOptions.None, (x, y) => x | y);
			_model.RunRegex();
		}
	}
}
