using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicalCMake
{
    public class ConnectorInfo
    {
        public CMakeArchRenderableTarget owner { get; private set; }
        public Connector connector { get; private set; }
        public ConnectorInfo(CMakeArchRenderableTarget owner, Connector connector)
        {
            this.owner = owner;
            this.connector = connector;
        }
    }

    public partial class Connector : Button
    {
        public string fr { get; set; }
        public ConnectorInfo info { get; private set; }
        public Connector(CMakeArchRenderableTarget owner) : base()
        {
            fr = "";

            BeginInit();
            Margin = new Thickness(0, 0, 0, 0);
            AllowDrop = true;
            IsHitTestVisible = true;
            Visibility = Visibility.Visible;

            Drop += Connector_Drop;
            Click += Connector_Click;
            ClickMode = ClickMode.Press;
            
            EndInit();

            info = new ConnectorInfo(owner, this);
        }

        private void Connector_Click(object sender, RoutedEventArgs e)
        {
            DragDrop.DoDragDrop(this, info, DragDropEffects.Link);
        }

        private void Connector_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetDataPresent(typeof(ConnectorInfo)))
            {
                var s = e.Data.GetData(typeof(ConnectorInfo)) as ConnectorInfo;
                if (s.connector == this) RaiseClickedEvent();
                else
                {
                    RaiseDroppedAsTargetEvent(s.connector); // i am being the target, source is the other connector
                    s.connector.RaiseDroppedAsSourceEvent(this); // s.connector is the source, i am the target(event source)
                }
            }
        }

    }




    // Connector Event Routing
    public partial class Connector
    {
        public static readonly RoutedEvent DroppedAsSourceEvent = EventManager.RegisterRoutedEvent(
            "DroppedAsSource", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Connector));

        public event RoutedEventHandler DroppedAsSource
        {
            add { AddHandler(DroppedAsSourceEvent, value); }
            remove { RemoveHandler(DroppedAsSourceEvent, value); }
        }

        protected void RaiseDroppedAsSourceEvent(Connector source)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Connector.DroppedAsSourceEvent, source);
            RaiseEvent(newEventArgs);
        }



        public static readonly RoutedEvent DroppedAsTargetEvent = EventManager.RegisterRoutedEvent(
            "DroppedAsTarget", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Connector));

        public event RoutedEventHandler DroppedAsTarget
        {
            add { AddHandler(DroppedAsTargetEvent, value); }
            remove { RemoveHandler(DroppedAsTargetEvent, value); }
        }

        protected void RaiseDroppedAsTargetEvent(Connector source)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Connector.DroppedAsTargetEvent, source);
            RaiseEvent(newEventArgs);
        }


        public static readonly RoutedEvent ClickedEvent = EventManager.RegisterRoutedEvent(
            "Clicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Connector));

        public event RoutedEventHandler Clicked
        {
            add { AddHandler(ClickedEvent, value); }
            remove { RemoveHandler(ClickedEvent, value); }
        }

        protected void RaiseClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Connector.ClickedEvent);
            RaiseEvent(newEventArgs);
        }
    }

}

