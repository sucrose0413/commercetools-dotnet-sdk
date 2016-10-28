﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;
using commercetools.Zones;

using NUnit.Framework;

using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ZoneManager class.
    /// </summary>
    [TestFixture]
    public class ZoneManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Zone> _testZones;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Project.Project> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            _project = projectTask.Result;

            _testZones = new List<Zone>();

            for (int i = 0; i < 5; i++)
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
                Task<Zone> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Zone zone = zoneTask.Result;

                Assert.NotNull(zone.Id);

                _testZones.Add(zone);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Zone zone in _testZones)
            {
                Task task = _client.Zones().DeleteZoneAsync(zone);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the ZoneManager.GetZoneByIdAsync method.
        /// </summary>
        /// <see cref="ZoneManager.GetZoneByIdAsync"/>
        [Test]
        public async Task ShouldGetZoneByIdAsync()
        {
            Zone zone = await _client.Zones().GetZoneByIdAsync(_testZones[0].Id);

            Assert.NotNull(zone);
            Assert.AreEqual(zone.Id, _testZones[0].Id);
        }

        /// <summary>
        /// Tests the ZoneManager.QueryZonesAsync method.
        /// </summary>
        /// <see cref="ZoneManager.QueryZonesAsync"/>
        [Test]
        public async Task ShouldQueryZonesAsync()
        {
            ZoneQueryResult result = await _client.Zones().QueryZonesAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.Zones().QueryZonesAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ZoneManager.CreateZoneAsync and ZoneManager.DeleteZoneAsync methods.
        /// </summary>
        /// <see cref="ZoneManager.CreateZoneAsync"/>
        /// <seealso cref="ZoneManager.DeleteZoneAsync(commercetools.Zones.Zone)"/>
        [Test]
        public async Task ShouldCreateAndDeleteZoneAsync()
        {
            ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
            Zone zone = await _client.Zones().CreateZoneAsync(zoneDraft);

            Assert.NotNull(zone.Id);

            string deletedZoneId = zone.Id;

            await _client.Zones().DeleteZoneAsync(zone);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.Zones().GetZoneByIdAsync(deletedZoneId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the ZoneManager.UpdateZoneAsync method.
        /// </summary>
        /// <see cref="ZoneManager.UpdateZoneAsync(commercetools.Zones.Zone, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateZoneAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newName = string.Concat("Test Zone ", Helper.GetRandomString(10));
            string newDescription = string.Concat("Test Description ", Helper.GetRandomString(10));
            
            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeName",
                    name = newName
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setDescription",
                    description = newDescription
                })
            );

            _testZones[1] = await _client.Zones().UpdateZoneAsync(_testZones[1], actions);

            Assert.NotNull(_testZones[1].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testZones[1].Name, newName);
                Assert.AreEqual(_testZones[1].Description, newDescription);
            }
        }
    }
}