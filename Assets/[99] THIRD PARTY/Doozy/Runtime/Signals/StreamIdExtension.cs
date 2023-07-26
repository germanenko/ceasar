// Copyright (c) 2015 - 2021 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.Navigate id, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), message);
        public static bool Send(StreamId.Navigate id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalSource, message);
        public static bool Send(StreamId.Navigate id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.Navigate id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.Navigate id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.Navigate id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.Navigate id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.Navigate id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Navigate), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.Swipe id, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), message);
        public static bool Send(StreamId.Swipe id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalSource, message);
        public static bool Send(StreamId.Swipe id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.Swipe id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.Swipe id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.Swipe id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.Swipe id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.Swipe id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Swipe), id.ToString(), signalValue, signalSender, message);   
    }

    public partial class StreamId
    {
        public enum Navigate
        {
            Down,
            Left,
            Right,
            Up
        }

        public enum Swipe
        {
            TopDown,
            Qqq,
            Down,
            Left,
            Right,
            Up
        }         
    }
}
