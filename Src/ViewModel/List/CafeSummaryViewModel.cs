﻿using System.Linq;
using System.Windows.Input;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class CafeSummaryViewModel : ViewModelBase
    {
        private string name;
        private double rating;
        private int numberOfVotes;
        private string address;
        private double latitude;
        private double longitude;
        private Photo photo;

        public string Name
        {
            get { return this.name; }
            set { this.Set(ref this.name, value); }
        }

        public double Rating
        {
            get { return this.rating; }
            set { this.Set(ref this.rating, value); }
        }

        public int NumberOfVotes
        {
            get { return this.numberOfVotes; }
            set { this.Set(ref this.numberOfVotes, value); }
        }

        public string Address
        {
            get { return this.address; }
            set { this.Set(ref this.address, value); }
        }

        public double Latitude
        {
            get { return this.latitude; }
            set { this.Set(ref this.latitude, value); }
        }

        public double Longitude
        {
            get { return this.longitude; }
            set { this.Set(ref this.longitude, value); }
        }

        public Photo Photo
        {
            get { return this.photo; }
            set { this.Set(ref this.photo, value); }
        }

        public ICommand Navigate { get; private set; }

        public CafeSummaryViewModel(Cafe model, INavigationService navigationService)
        {
            this.Navigate = new NavigateToCafeDetailsCommand(model, navigationService);
            this.Populate(model);
            this.PopulatePhoto(model);
        }

        private void Populate(Cafe model)
        {
            this.Name = model.Name;
            this.Address = model.Address;
            this.Longitude = model.Longitude;
            this.Latitude = model.Latitude;
            this.Rating = (model.CoffeeRating + model.AtmosphereRating) / 2;
            this.NumberOfVotes = model.NumberOfVotes;
        }

        private void PopulatePhoto(Cafe model)
        {
            this.photo = model.Photos
                                .OrderByDescending(p => p.NumberOfVotes)
                                .FirstOrDefault();
        }
    }
}
