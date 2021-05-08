﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading.Tasks;

namespace Sensors.Temperature.AnalogTemperature_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            // configure our AnalogTemperature sensor
            analogTemperature = new AnalogTemperature (
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );

            //==== IObservable Pattern with an optional notification filter.
            // Example that uses an IObersvable subscription to only be notified
            // when the temperature changes by at least a degree.
            var consumer = AnalogTemperature.CreateObserver(
                handler: result => {
                    Console.WriteLine($"Observer filter satisfied: {result.New.Celsius:N2}C, old: {result.Old?.Celsius:N2}C");
                },
                // only notify if the change is greater than 0.5°C
                // this filter is a predicate, so if you want to get notified,
                // is needs to return true.
                filter: result => {
                    // if it's not null, do a comparison
                    if (result.Old is { } old) {
                        return (result.New - old).Abs().Celsius > 0.5;
                    } // if the old result is null, it's the first time, so we want to get notified
                    else {
                        Console.WriteLine("Filter: result.old is null");
                        return true;
                    }
                }
                // if you want to always get notified, pass null for the filter:
                //filter: null
            );
            analogTemperature.Subscribe(consumer);

            //==== Classic Events Pattern
            // classical .NET events can also be used:
            analogTemperature.TemperatureUpdated += (object sender, IChangeResult<Meadow.Units.Temperature> result) => {
                Console.WriteLine($"Temp Changed, temp: {result.New.Celsius:N2}C, old: {result.Old?.Celsius:N2}C");
            };

            //==== One-off reading use case/pattern
            //ReadTemp().Wait();

            // Spin up the sampling thread so that events are raised and
            // IObservable notifications are sent.
            analogTemperature.StartUpdating();
        }

        protected async Task ReadTemp()
        {
            var temperature = await analogTemperature.Read();
            Console.WriteLine($"Initial temp: {temperature.New.Celsius:N2}C");
        }
    }
}