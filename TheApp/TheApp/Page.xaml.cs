
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reactive.Concurrency;
using System.Reactive.Threading;

using System.Reactive.Linq;
using System.Net.Http;

namespace TheApp
{
    public class Monkey : ReactiveObject
    {
        public string Name { get; set; }
    }
	public partial class Page : ContentPage
	{

        HttpClient client = new HttpClient();

		public Page ()
		{
			InitializeComponent ();



            //uncomment this to use an ObservableCollection instead and the items will show up in the list
            //without a problem
            //ObservableCollection<Monkey> Monkies = new ObservableCollection<Monkey>();
            ReactiveList<Monkey> Monkies = new ReactiveList<Monkey>();
            
            
            ObservableCollection<Monkey> Monkies2 = new ObservableCollection<Monkey>();


            Monkies.CollectionChanged += (x, y) =>
                {
                    if(y.NewItems != null)
                    {
                        foreach (Monkey item in y.NewItems)
                        {
                            if(!Monkies2.Contains(item))
                                Monkies2.Add(item);
                        }
                    }
                };

            int i = 0;

            bool isrunning = false;

            System.Reactive.Linq.Observable.Interval(TimeSpan.FromSeconds(2))
                .Where(_ => !isrunning)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(async x =>
                {
                    try
                    {
                        isrunning = true;
                        var there = Monkies.ToList();
                        Monkies.Clear();
                        there.ForEach(y => Monkies.Add(y));
                        await client.GetStringAsync("http://www.google.com");
                        Monkey george = new Monkey() { Name = string.Format("Jungle{0}", i) };

                        Monkies.Add(george);
                        i++;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        isrunning = false;
                    }
                });
            theView.ItemsSource = Monkies;
            theViewPart2.ItemsSource = Monkies2;

            
		}
	}
}
