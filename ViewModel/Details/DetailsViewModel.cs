﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class DetailsViewModel : ViewModelBase, INavigationListener
    {
        private readonly IDataService dataService;

        private double coffeeRating;
        private double atmosphereRating;
        private int numberOfVotes;

        public string Name { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public string Address { get; private set; }

        public string PostCode { get; private set; }

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

        public ObservableCollection<Comment> Comments { get; private set; }
        
        public NewComment NewComment { get; set; }

        public DetailsViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
            this.Comments = new ObservableCollection<Comment>();
            this.NewComment = new NewComment(this.dataService);
        }

        public Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            var cafeId = (int) parameters["Id"];

            var detailsTask = this.GetCafe(cafeId).ContinueWith(task => Populate(task.Result));
            var commentsTask = this.dataService.GetCafeComments(cafeId).ContinueWith(task => Populate(task.Result));
            return Task.WhenAll(detailsTask, commentsTask);
        }

        private void Populate(Cafe cafe)
        {
            // TODO: Unassign during navigate away?
            this.NewComment.AssociatedCafe = cafe;

            this.Name = cafe.Name;
            this.RaisePropertyChanged(() => this.Name);

            this.Address = cafe.Address;
            this.RaisePropertyChanged(() => this.Address);

            this.PostCode = cafe.PostCode;
            this.RaisePropertyChanged(() => this.PostCode);

            this.Longitude = cafe.Longitude;
            this.RaisePropertyChanged(() => this.Longitude);

            this.Latitude = cafe.Latitude;
            this.RaisePropertyChanged(() => this.Latitude);

            this.CoffeeRating = cafe.CoffeeRating;
            this.AtmosphereRating = cafe.AtmosphereRating;
            this.NumberOfVotes = cafe.NumberOfVotes;
        }

        private void Populate(IEnumerable<Comment> comments)
        {
            var sorted = comments
                .OrderByDescending(comment => comment.CreatedDate);
            foreach (var comment in sorted)
            {
                this.Comments.Add(comment);
            }
        }

        private async Task<Cafe> GetCafe(int cafeId)
        {
            var cafes = await this.dataService.GetAllCafes();
            var cafe = cafes.FirstOrDefault(c => c.Id == cafeId);
            if (cafe == null)
            {
                throw new ArgumentException(
                    string.Format("Could not retrieve cafe {0} from data service", cafeId));
            }
            return cafe;
        }
    }
}
