﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class DetailsViewModel : ViewModelBase, INavigationListener
    {
        private readonly IDataService dataService;
        private readonly INavigationService navigationService;
        private readonly IMapLauncher mapLauncher;
        private readonly IShareSource shareSource;

        private string cafeId;
        private double coffeeRating;
        private double atmosphereRating;
        private int numberOfVotes;

        public string Name { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public string Address { get; private set; }

        public double CoffeeRating
        {
            get { return this.coffeeRating; }
            private set { this.Set(ref this.coffeeRating, value); }
        }

        public double AtmosphereRating
        {
            get { return this.atmosphereRating; }
            private set { this.Set(ref this.atmosphereRating, value); }
        }

        public int NumberOfVotes
        {
            get { return this.numberOfVotes; }
            private set { this.Set(ref this.numberOfVotes, value); }
        }

        public ObservableCollection<PhotoViewModel> Photos { get; private set; }
        
        public ObservableCollection<ReviewViewModel> Reviews { get; private set; }

        public RelayCommand ShowDirections { get; private set; }
        
        public RelayCommand NavigateToMap { get; private set; }

        public ShareCafeCommand Share { get; private set; }

        public DetailsViewModel(IDataService dataService, INavigationService navigationService, IMapLauncher mapLauncher, IShareSource shareSource)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.mapLauncher = mapLauncher;
            this.shareSource = shareSource;
            this.Photos = new ObservableCollection<PhotoViewModel>();
            this.Reviews = new ObservableCollection<ReviewViewModel>();
            this.ShowDirections = new RelayCommand(this.OnShowDirectionsExecuted);
            this.NavigateToMap = new RelayCommand(this.OnNavigateToMapExecuted);
            this.Share = new ShareCafeCommand(this.shareSource);
            new ReviewViewModel(this.dataService);

#if DEBUG
            if (this.IsInDesignMode)
            {
                this.cafeId = "2";
                this.Populate();
            }
#endif
        }

        public Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            this.cafeId = (string) parameters["Id"];
            return this.Populate();
        }

        public void OnNavigatedFrom()
        {
            this.Reviews.Clear();
            this.Photos.Clear();
            this.shareSource.IsEnabled = false;
            this.Share.Cafe = null;
        }

        private Task Populate()
        {
            var detailsTask = this.GetCafe()
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            var reviewsTask = this.dataService.GetCafeReviews(cafeId)
                .ContinueWith(task => Populate(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
            return Task.WhenAll(detailsTask, reviewsTask);
        }

        private void Populate(Cafe cafe)
        {
            this.Name = cafe.Name;
            this.RaisePropertyChanged(() => this.Name);

            this.Address = string.Format("{0}, {1}", cafe.Address, cafe.PostCode);
            this.RaisePropertyChanged(() => this.Address);

            this.Longitude = cafe.Longitude;
            this.RaisePropertyChanged(() => this.Longitude);

            this.Latitude = cafe.Latitude;
            this.RaisePropertyChanged(() => this.Latitude);

            this.CoffeeRating = cafe.CoffeeRating;
            this.AtmosphereRating = cafe.AtmosphereRating;
            this.NumberOfVotes = cafe.NumberOfVotes;

            foreach (var photo in cafe.Photos.OrderByDescending(p => p.NumberOfVotes))
            {
                this.Photos.Add(new PhotoViewModel(photo));
            }

            this.shareSource.IsEnabled = true;
            this.Share.Cafe = cafe;
        }

        private void Populate(IEnumerable<Review> reviews)
        {
            var sorted = reviews
                .OrderByDescending(review => review.SubmittedDate);
            foreach (var review in sorted)
            {
                if (!string.IsNullOrEmpty(review.Comment))
                {
                    this.Reviews.Add(this.CreateReviewViewModel(review));
                }
            }
        }

        private async Task<Cafe> GetCafe()
        {
            var cafes = await this.dataService.GetAllCafes();
            var cafe = cafes.FirstOrDefault(c => c.Id == this.cafeId);
            if (cafe == null)
            {
                throw new ArgumentException(
                    string.Format("Could not retrieve cafe {0} from data service", cafeId));
            }
            return cafe;
        }
        private void OnShowDirectionsExecuted()
        {
            this.mapLauncher.Launch(this.Longitude, this.Latitude, this.Name);
        }

        private void OnNavigateToMapExecuted()
        {
            this.navigationService.NavigateToMap(this.cafeId);
        }

        private ReviewViewModel CreateReviewViewModel(Review model)
        {
            var viewModel = new ReviewViewModel(this.dataService);
            viewModel.Initialize(model);
            return viewModel;
        }
    }
}
