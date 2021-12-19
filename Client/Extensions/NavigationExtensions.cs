using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AuthClient.Client.Extensions
{
    public class NavigationExtensions : NavigationManager, IDisposable
    {
        private const int MinHistorySize = 256;
        private const int AdditionalHistorySize = 64;
        private readonly NavigationManager _navigationManager;
        private readonly List<string> _history;

        public NavigationExtensions(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _history = new List<string>(MinHistorySize + AdditionalHistorySize);
            _history.Add(_navigationManager.Uri);
            _navigationManager.LocationChanged += OnLocationChanged;
        }

        /// <summary>
        /// Переход по адресу 
        /// </summary>
        /// <param name="url">Путь</param>
        public void NavigateTo(string url)
        {
            _navigationManager.NavigateTo(url);
        }

        /// <summary>
        /// Возвращает true если возможно перейте на предыдущий путь
        /// </summary>
        public bool CanNavigateBack => _history.Count >= 2;

        /// <summary>
        /// Перейти назад если возможно
        /// </summary>
        public void NavigateBack()
        {
            if (!CanNavigateBack) return;
            var backPageUrl = _history[^2];
            _history.RemoveRange(_history.Count - 2, 2);
            _navigationManager.NavigateTo(backPageUrl);
        }

        // Helpers
        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            EnsureSize();
            _history.Add(e.Location);
        }

        private void EnsureSize()
        {
            if (_history.Count < MinHistorySize + AdditionalHistorySize) return;
            _history.RemoveRange(0, _history.Count - MinHistorySize);
        }

        public void Dispose()
        {
            _navigationManager.LocationChanged -= OnLocationChanged;
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            throw new NotImplementedException();
        }
    }
}
