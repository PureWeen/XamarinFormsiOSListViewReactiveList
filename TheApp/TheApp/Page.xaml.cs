
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
using System.Collections.Specialized;

namespace TheApp
{
    public class Monkey 
    {
        public string Name { get; set; }
    }

    public  class MyCollection : Collection<Monkey>, INotifyCollectionChanged
    {

        public new void Add(Monkey item)
        {
            base.Add(item);
            CollectionChangedEventManager.DeliverEvent(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

        }
        protected override void ClearItems()
        {
            base.ClearItems();

            CollectionChangedEventManager.DeliverEvent(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void SetItem(int index, Monkey item)
        {
            base.SetItem(index, item);


        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { CollectionChangedEventManager.AddHandler(this, value); }
            remove { CollectionChangedEventManager.RemoveHandler(this, value); }
        }

        
    }

	public partial class Page : ContentPage
	{

        ObservableCollection<Monkey> Monkies = new ObservableCollection<Monkey>();
        MyCollection Monkies2 = new MyCollection();
        HttpClient client = new HttpClient();

		public Page ()
		{
			InitializeComponent ();



            //uncomment this to use an ObservableCollection instead and the items will show up in the list
            //without a problem
            //ObservableCollection<Monkey> Monkies = new ObservableCollection<Monkey>();



            Monkies.CollectionChanged += Monkies_CollectionChanged;

            int i = 0;

            bool isrunning = false;

            
            System.Reactive.Linq.Observable.Interval(TimeSpan.FromSeconds(2))
                .Where(_ => !isrunning)
                .Subscribe(x =>
                {
                    isrunning = true;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                
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
                });

            theView.ItemsSource = Monkies;
            theViewPart2.ItemsSource = Monkies2;

            
		}

        void Monkies_CollectionChanged(object x, NotifyCollectionChangedEventArgs y)
        {
                if(y.NewItems != null)
                {
                    foreach (Monkey item in y.NewItems)
                    {
                        if(!Monkies2.Contains(item))
                            Monkies2.Add(item);
                    }
                }
        }
	}
}
