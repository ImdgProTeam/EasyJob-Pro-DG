﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyJob_ProDG.ModelTests.Cargo
{
    [TestClass]
    public class SegregationTests : Segregation
    {
        static Segregation segregation = new Segregation();
        PrivateObject segregationObject = new PrivateObject(segregation);
        private static List<Dg> dgList;
        public static ShipProfile ship = ShipProfile.GetDefaultShipProfile();

        public TestContext TestContext { get; set; }

        ///Method defines Bay, Row and Tier
        static void DefineContainerLocation(Dg dg)
        {
            dg.HoldNr = ship.DefineCargoHoldNumberOfBay(dg.Bay);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //ship profile
            ship.Row00Exists = true;
            ship.NumberOfHolds = 3;
            ship.CargoHolds = new List<CargoHold>()
                {
                    new CargoHold(1, 1, 7),
                    new CargoHold(2, 9, 15),
                    new CargoHold(3, 17, 23)
            };
            ship.SetThisShipProfileToShip();
            

            //Creating dg list
            dgList = new List<Dg>();

            Dg dg1 = new Dg()
            {
                DgClass = "2.1",
                Location = "100182",
                ContainerNumber = "container1",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = true

            };
            DefineContainerLocation(dg1);
            dgList.Add(dg1);

            Dg dg2 = new Dg()
            {
                DgClass = "3",
                Location = "100382",
                ContainerNumber = "container2",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = true
            };
            DefineContainerLocation(dg2);
            dgList.Add(dg2);

            Dg dg3 = new Dg()
            {
                DgClass = "3",
                Location = "100582",
                ContainerNumber = "container3",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = true
            };
            DefineContainerLocation(dg3);
            dgList.Add(dg3);

            Dg dg4 = new Dg()
            {
                DgClass = "2.1",
                DgSubClass = "2.3",
                Location = "190008",
                ContainerNumber = "container4",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = false

            };
            DefineContainerLocation(dg4);
            dgList.Add(dg4);

            Dg dg5 = new Dg()
            {
                DgClass = "6.1",
                DgSubClass = "8",
                Location = "180002",
                ContainerNumber = "container5",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = true

            };
            DefineContainerLocation(dg5);
            dgList.Add(dg5);

            Dg dg6 = new Dg()
            {
                DgClass = "6.2",
                Location = "151212",
                ContainerNumber = "container6",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = false

            };
            DefineContainerLocation(dg6);
            dgList.Add(dg6);

            Dg dg7 = new Dg()
            {
                DgClass = "3",
                DgSubClass = "8",
                Location = "010502",
                ContainerNumber = "container7",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = false

            };
            DefineContainerLocation(dg7);
            dgList.Add(dg7);


        }

        public void TestDgValidValues(Dg unit, ref int result, byte holdNr = 2, int dgClassCount = 1, bool isUnderdeck = false, byte size = 40)
        {

            if (string.Equals(unit.DgClass, unit.AllDgClasses[0])) result++;
            Debug.WriteLine(string.Format("Unit dg class is {0}", unit.DgClass));
            Debug.WriteLine(string.Format("All dg classes: {0}", unit.AllDgClasses));
            Debug.WriteLine(string.Format("First dg class: {0}", unit.AllDgClasses[0]));


            if (unit.AllDgClasses.Count == dgClassCount) result++;
            Debug.WriteLine("All dg classes count is {0}", unit.AllDgClasses.Count);

            Debug.WriteLine(string.Format("Container location is {0}", unit.Location));
            if (unit.Bay == Convert.ToByte(unit.Location.Replace(" ", "").Substring(0, 3))) result++;
            Debug.WriteLine("Bay = {0}, substring = {1}", unit.Bay, Convert.ToByte(unit.Location.Replace(" ", "").Substring(0, 3)));
            if (unit.Row == Convert.ToByte(unit.Location.Replace(" ", "").Substring(3, 2))) result++;
            Debug.WriteLine("Row = {0}, substring = {1}", unit.Row, Convert.ToByte(unit.Location.Replace(" ", "").Substring(3, 2)));
            if (unit.Tier == Convert.ToByte(unit.Location.Replace(" ", "").Substring(5, 2))) result++;
            Debug.WriteLine("Tier = {0}, substring = {1}", unit.Tier, Convert.ToByte(unit.Location.Replace(" ", "").Substring(5, 2)));
            Debug.WriteLine("Dg IsUnderdeck: {0}", unit.IsUnderdeck);
            if (unit.IsUnderdeck == isUnderdeck) result++;
            Debug.WriteLine("Dg is in hold {0}", unit.HoldNr);
            if (unit.HoldNr == holdNr) result++;
            Debug.WriteLine("Dg container Size is {0}", unit.Size);
            if (unit.Size == size) result++;


            Debug.WriteLine("Result = {0}", result);
            Assert.AreEqual(result, 8);

        }

        [TestMethod]
        public void TestDg1ValidValues()
        {
            int result = 0;
            Dg unit = dgList[0];

            TestDgValidValues(unit, ref result);
            Assert.AreEqual(result, 8);

        }

        [TestMethod]
        public void TestDg4ValidValues()
        {
            int result = 0;
            Dg unit = dgList[3];

            TestDgValidValues(unit, ref result, holdNr: 3, dgClassCount: 2, isUnderdeck: true, size: 20);
            Assert.AreEqual(result, 8);

        }

        [TestMethod()]
        public void SegregateNoSegregatorNoExeptionConflicted()
        {
            bool result;

            Debug.WriteLine("Test two units with no segregator class and no segregator exception");
            result = Segregation.Segregate(dgList[0], dgList[1], ship);

            Assert.IsTrue(result);

        }

        [TestMethod]
        ///Class 3 one container space from class 2.1 on deck
        public void SegregateNoSegregatorNoExeptionNoConflict()
        {
            bool result;

            Debug.WriteLine("Test two units with no segregator class and no segregator exception");
            int segrLevel = Segregation.GetSegregationLevelFromTable(dgList[0].DgClass, dgList[2].DgClass);
            Debug.WriteLine("Segregation level between two containers: {0}", segrLevel);
            result = Segregation.Segregate(dgList[0], dgList[2], ship);
            if (result == false) result = Segregation.Segregate(dgList[1], dgList[2], ship);

            Assert.IsFalse(result);

        }

        [TestMethod]
        ///Open versus IsClosed in the same vertical line (class 2.1 versus 8)
        public void SegregateOpenVersusIsClosedNoSegregatorNoExeptionConflict()
        {
            bool result;

            Debug.WriteLine("Test two units with no segregator class and no segregator exception");
            int segrLevel = Segregation.GetSegregationLevelFromTable(dgList[3].DgClass, dgList[4].DgClass);
            Debug.WriteLine("Segregation level between two containers: {0}", segrLevel);
            result = Segregation.Segregate(dgList[3], dgList[4], ship);
            //if (result == false) result = Segregation.Segregate(dgList[1], dgList[2], ship);

            Assert.IsFalse(result);

        }

        [TestMethod]
        ///Containers with segLevel 3 in adjacent holds (class 8 versus 6.2)
        public void SegregateLevel3InAdjacentHoldsNoSegregatorNoExeptionConflict()
        {
            bool result;
            Dg a = dgList[4],
                b = dgList[5],
                c = dgList[6];


            Debug.WriteLine("Test two units with no segregator class and no segregator exception");
            int segrLevel = Segregation.GetSegregationLevelFromTable(a.DgSubClassArray[0], b.DgClass);
            Debug.WriteLine("Segregation level between two containers: {0}", segrLevel);
            result = Segregation.Segregate(a, b, ship);
            if (result == false)
            {
                segrLevel = Segregation.GetSegregationLevelFromTable(b.DgClass, c.DgClass);
                Debug.WriteLine("First test passed succesfully - IsClosed versus open in djacent holds.\nSegregation level between two containers: {0}", segrLevel);
                Debug.WriteLine("Holds are {0} and {1}", b.HoldNr, c.HoldNr);
                Debug.WriteLine("IsClosed: {0} - {1}", b.IsClosed, c.IsClosed);
                Debug.WriteLine("IsUnderdeck: {0} - {1}", b.IsUnderdeck, c.IsUnderdeck);
                result = !Segregation.Segregate(b, c, ship);
                Debug.WriteLine("Result is {0}", result);
            }

            Assert.IsFalse(result);

        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "ConflictsInDgList.xml", "Combination", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CheckSegregationFromXmlList()
        {
            Dg dgA = new Dg();
            dgA.DgClass = Convert.ToString(TestContext.DataRow["DgA.Dgclass"]);
            dgA.DgSubClass = Convert.ToString(TestContext.DataRow["DgA.DgSubClass1"]);
            dgA.DgSubClass = Convert.ToString(TestContext.DataRow["DgA.DgSubClass2"]);
            dgA.Location = Convert.ToString(TestContext.DataRow["DgA.CntrLocation"]);
            dgA.ContainerNumber = Convert.ToString(TestContext.DataRow["DgA.cntrNr"]);
            dgA.IsClosed = Convert.ToBoolean(TestContext.DataRow["DgA.closed"]);
            string SegrClassA = Convert.ToString(TestContext.DataRow["DgA.segregatorClass"]);
            dgA.SegregatorClass = SegrClassA == "null" || SegrClassA == "" ? null : SegrClassA;
            string SegregatorExceptionClass = Convert.ToString(TestContext.DataRow["DgA.segregatorException.segrClass"]);
            if (!String.IsNullOrEmpty(SegregatorExceptionClass) && SegregatorExceptionClass != "null")
            {
                Debug.WriteLine("Segregaton exception exists!");
                byte SegregatorExceptionLevel = Convert.ToByte(TestContext.DataRow["DgA.segregatorException.segrLevel"]);
                dgA.SegregatorException = new SegregatorException(SegregatorExceptionClass, SegregatorExceptionLevel);
            }

            Dg dgB = new Dg();
            dgB.DgClass = Convert.ToString(TestContext.DataRow["dgB.Dgclass"]);
            dgB.DgSubClass = Convert.ToString(TestContext.DataRow["dgB.DgSubClass1"]);
            dgB.DgSubClass = Convert.ToString(TestContext.DataRow["dgB.DgSubClass2"]);
            dgB.Location = Convert.ToString(TestContext.DataRow["dgB.CntrLocation"]);
            dgB.ContainerNumber = Convert.ToString(TestContext.DataRow["dgB.cntrNr"]);
            dgB.IsClosed = Convert.ToBoolean(TestContext.DataRow["dgB.closed"]);
            string SegrClassB = Convert.ToString(TestContext.DataRow["DgB.segregatorClass"]);
            dgB.SegregatorClass = SegrClassB == "null" || SegrClassB == "" ? null : SegrClassB;
            SegregatorExceptionClass = Convert.ToString(TestContext.DataRow["DgB.segregatorException.segrClass"]);
            if (!string.IsNullOrEmpty(SegregatorExceptionClass) && SegregatorExceptionClass != "null")
            {
                Debug.WriteLine("Segregaton exception exists!");
                byte SegregatorExceptionLevel = Convert.ToByte(TestContext.DataRow["DgB.segregatorException.segrLevel"]);
                dgB.SegregatorException = new SegregatorException(SegregatorExceptionClass, SegregatorExceptionLevel);
            }


            Debug.WriteLine("Dg A {0} in {1} (IsClosed: {2})", dgA.ContainerNumber, dgA.Location, dgA.IsClosed);
            Debug.WriteLine("Class {0} ({1})", dgA.DgClass, dgA.DgSubClass);
            Debug.WriteLine("Segregator class: {0}, SegregatorException: {1} (class {2} level {3})", dgA.SegregatorClass, dgA.SegregatorException != null,
                dgA.SegregatorException != null ? dgA.SegregatorException.SegrClass : "NA",
                dgA.SegregatorException != null ? dgA.SegregatorException.SegrCase.ToString() : "NA");
            Debug.WriteLine("Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", dgA.Bay, dgA.Row, dgA.Tier, dgA.HoldNr, dgA.IsUnderdeck);
            Debug.WriteLine("");
            Debug.WriteLine("Dg B {0} in {1} (IsClosed: {2})", dgB.ContainerNumber, dgB.Location, dgB.IsClosed);
            Debug.WriteLine("Class {0} ({1})", dgB.DgClass, dgB.DgSubClass);
            Debug.WriteLine("Segregator class: {0}, SegregatorException: {1} (class {2} level {3})", dgB.SegregatorClass, dgB.SegregatorException != null,
                dgB.SegregatorException != null ? dgB.SegregatorException.SegrClass : "NA",
                dgB.SegregatorException != null ? dgB.SegregatorException.SegrCase.ToString() : "NA");
            Debug.WriteLine("Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", dgB.Bay, dgB.Row, dgB.Tier, dgB.HoldNr, dgB.IsUnderdeck);

            Debug.WriteLine("\nDefine container location:");
            DefineContainerLocation(dgA);
            Debug.WriteLine("DG A: Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", dgA.Bay, dgA.Row, dgA.Tier, dgA.HoldNr, dgA.IsUnderdeck);
            DefineContainerLocation(dgB);
            Debug.WriteLine("DG B: Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", dgB.Bay, dgB.Row, dgB.Tier, dgB.HoldNr, dgB.IsUnderdeck);
            Debug.WriteLine("");

            foreach (string a in dgA.AllDgClasses)
            {
                foreach (string b in dgB.AllDgClasses)
                {
                    var segLevel = Segregation.GetSegregationLevelFromTable(a, b);
                    Debug.WriteLine("Segregation level between class {0} and {1}:   {2}", a, b, segLevel);
                }
            }

            if (dgA.SegregatorClass != null)
            {
                foreach (var b in dgB.AllDgClasses)
                {
                    var segLevel = Segregation.GetSegregationLevelFromTable(dgA.SegregatorClass, b);
                    Debug.WriteLine("Segregation level between segregator class {0} and class {1}:   {2}", dgA.SegregatorClass, b, segLevel);
                }
                if (dgB.SegregatorClass != null)
                {
                    var segLevel = Segregation.GetSegregationLevelFromTable(dgA.SegregatorClass, dgB.SegregatorClass);
                    Debug.WriteLine("Segregation level between segregator class {0} and segregator class {1}:   {2}", dgA.SegregatorClass, dgB.SegregatorClass, segLevel);
                }
            }
            else if (dgB.SegregatorClass != null)
            {
                foreach (var a in dgA.AllDgClasses)
                {
                    var segLevel = Segregation.GetSegregationLevelFromTable(dgB.SegregatorClass, a);
                    Debug.WriteLine("Segregation level between segregator class {0} and class {1}:   {2}", dgB.SegregatorClass, a, segLevel);
                }
                if (dgA.SegregatorClass != null)
                {
                    var segLevel = Segregation.GetSegregationLevelFromTable(dgA.SegregatorClass, dgB.SegregatorClass);
                    Debug.WriteLine("Segregation level between segregator class {0} and segregator class {1}:   {2}", dgB.SegregatorClass, dgA.SegregatorClass, segLevel);
                }
            }

            Debug.WriteLine("");
            for (int i = 0; i < 5; i++)
            {
                bool segCheck = Segregation.SegregationConflictCheck(dgA, dgB, (byte)i);
                Debug.WriteLine("Segregation check at level {0} result is {1}", i, segCheck);
            }

            Debug.WriteLine("---------------------------");
            bool ExpectedResult = Convert.ToBoolean(TestContext.DataRow["ExpectedResult"]);
            Debug.WriteLine("\nExpected result: {0}", ExpectedResult);


            bool result = Segregation.Segregate(dgA, dgB, ship);
            Debug.WriteLine("Segregation result: {0}", result);



            Assert.AreEqual(ExpectedResult, result);
        }

        [TestMethod]
        public void SegregateAssistant()
        {
            Dg a = new Dg()
            {
                DgClass = "4.1",
                Location = "220088",
                ContainerNumber = "containerA",
                SegregatorClass = "1.3G",
                SegregatorException = new SegregatorException("3", 2),
                IsClosed = true

            };
            a.DgSubClass = "6.1";
            a.DgSubClass = "8";
            DefineContainerLocation(a);

            Dg b = new Dg()
            {
                DgClass = "3",
                Location = "180090",
                ContainerNumber = "containerB",
                SegregatorClass = null,
                SegregatorException = null,
                IsClosed = true

            };
            DefineContainerLocation(b);

            Debug.WriteLine("Dg A {0} in {1} (IsClosed: {2})", a.ContainerNumber, a.Location, a.IsClosed);
            Debug.WriteLine("Class {0} ({1})", a.DgClass, a.DgSubClass);
            Debug.WriteLine("Segregator class: {0}, SegregatorException: {1} ({2} level {3})", a.SegregatorClass, a.SegregatorException != null,
                a.SegregatorException != null ? a.SegregatorException.SegrClass : "NA",
                a.SegregatorException != null ? a.SegregatorException.SegrCase.ToString() : "NA");
            Debug.WriteLine("DG A: Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", a.Bay, a.Row, a.Tier, a.HoldNr, a.IsUnderdeck);
            Debug.WriteLine("");
            Debug.WriteLine("Dg B {0} in {1} (IsClosed: {2})", b.ContainerNumber, b.Location, b.IsClosed);
            Debug.WriteLine("Class {0} ({1})", b.DgClass, b.DgSubClass);
            Debug.WriteLine("Segregator class: {0}, SegregatorException: {1} ({2} level {3})", b.SegregatorClass, b.SegregatorException != null,
                b.SegregatorException != null ? b.SegregatorException.SegrClass : "NA",
                b.SegregatorException != null ? b.SegregatorException.SegrCase.ToString() : "NA");
            Debug.WriteLine("DG B: Bay {0} Row {1} Tier {2} hold {3} IsUnderdeck {4}", b.Bay, b.Row, b.Tier, b.HoldNr, b.IsUnderdeck);


            bool _conf = false;
            byte seglevel = 0,
                _seglevel,
                _seglevel5 = 0;

            //case dg a has NO segregator class
            if (a.SegregatorClass == null)
            {
                Debug.WriteLine("A has NO segregator class");
                //case there are no segregator classes
                if (b.SegregatorClass == null)
                {
                    Debug.WriteLine("B has NO segregator class");
                    Debug.WriteLine("Checking compatibility of each dg class in A: {0}", a.AllDgClasses);
                    Debug.WriteLine("Control value of DgClass {0}", a.AllDgClasses[0]);
                    foreach (string cla in a.AllDgClasses)
                    {
                        Debug.WriteLine("Checking compatibility with B all dg classes: {0}", b.AllDgClasses);
                        Debug.WriteLine("Control value of DgClass {0}", b.AllDgClasses[0]);
                        foreach (string clb in b.AllDgClasses)
                        {
                            _seglevel = (byte)Segregation.GetSegregationLevelFromTable(cla, clb);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }

                }

                //case only dg b has a segregator class
                else
                {
                    Debug.WriteLine("B has segregator class {0}", b.SegregatorClass);
                    if (b.SegregatorException == null)
                    {
                        Debug.WriteLine("Checking compatibility with each dg class in A: {0}", a.AllDgClasses);
                        Debug.WriteLine("Control value of DgClass {0}", a.AllDgClasses[0]);
                        foreach (string cla in a.AllDgClasses)
                        {
                            _seglevel = (byte)Segregation.GetSegregationLevelFromTable(cla, b.SegregatorClass);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //b.SegregatorException != null
                    {
                        Debug.WriteLine("B has segregator exception: class {0} - level {1}",
                            b.SegregatorException.SegrClass, b.SegregatorException.SegrCase);
                        Debug.WriteLine("Checking compatibility with each dg class in A: {0}", a.AllDgClasses);
                        Debug.WriteLine("Control value of DgClass {0}", a.AllDgClasses[0]);
                        foreach (string cla in a.AllDgClasses)
                        {
                            if (cla == b.SegregatorException.SegrClass) _seglevel = b.SegregatorException.SegrCase; //except for class ... 
                            else if (b.SegregatorException.SegrClass == "1" && cla.StartsWith("1")) _seglevel = b.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregation.GetSegregationLevelFromTable(cla, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregation.GetSegregationLevelFromTable(cla, b.DgClass); //segregation as for the primary hazard 7.2.8

                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;

                        }
                    }
                }
            }

            //case dg a has a segregator class
            else
            {
                Debug.WriteLine("A has segregator class {0}", a.SegregatorClass);
                //case ONLY dg a has a segregator class
                if (b.SegregatorClass == null)
                {
                    Debug.WriteLine("B has NO segregator class");
                    if (a.SegregatorException == null)
                    {

                        Debug.WriteLine("Checking compatibility with B all dg classes: {0}", b.AllDgClasses);
                        Debug.WriteLine("Control value of DgClass {0}", b.AllDgClasses[0]);
                        foreach (string clb in b.AllDgClasses)
                        {
                            _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, clb);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //a.SegregatorException != null
                    {
                        foreach (string clb in b.AllDgClasses)
                        {
                            if (clb == a.SegregatorException.SegrClass) _seglevel = a.SegregatorException.SegrCase; //except for class ... 
                            else if (a.SegregatorException.SegrClass == "1"
                                && clb.StartsWith("1")) _seglevel = a.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, clb);
                            if (_seglevel == 99) _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.DgClass, clb); //segregation as for the primary hazard 7.2.8

                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                }
                //case both dgs have segregator classes
                else
                {
                    Debug.WriteLine("B has segregator class {0}", b.SegregatorClass);
                    if (a.SegregatorException == null)
                    {
                        //both have NO SegregatorException
                        if (b.SegregatorException == null)
                        {
                            seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, b.SegregatorClass);
                        }
                        else //b.SegregatorException != null
                        {
                            if (a.SegregatorClass == b.SegregatorException.SegrClass) _seglevel = b.SegregatorException.SegrCase; //except for class ... 
                            else if (b.SegregatorException.SegrClass == "1"
                                && a.SegregatorClass.StartsWith("1")) _seglevel = b.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, b.DgClass); //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //a.SegregatorException != null
                    {
                        if (b.SegregatorException == null)
                        {
                            if (a.SegregatorException.SegrClass == b.SegregatorClass) _seglevel = a.SegregatorException.SegrCase;
                            else if (a.SegregatorException.SegrClass == "1"
                                && b.SegregatorClass.StartsWith("1")) _seglevel = a.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.DgClass, b.SegregatorClass); //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                        else //both have SegregatorException
                        {
                            byte s1 = 0, s2 = 0;
                            if (a.SegregatorClass == b.SegregatorException.SegrClass
                                || a.SegregatorException.SegrClass == b.SegregatorClass)
                            {
                                byte l1 = a.SegregatorClass == b.SegregatorException.SegrClass
                                    ? a.SegregatorException.SegrCase : (byte)0;
                                byte l2 = a.SegregatorException.SegrClass == b.SegregatorClass
                                    ? b.SegregatorException.SegrCase : (byte)0;
                                _seglevel = l1 > l2 ? l1 : l2;
                            }

                            else if (a.SegregatorException.SegrClass == "1" && b.SegregatorClass.StartsWith("1")
                                || b.SegregatorException.SegrClass == "1" && a.SegregatorClass.StartsWith("1"))
                            {
                                s1 = a.SegregatorException.SegrClass == "1" && b.SegregatorClass.StartsWith("1")
                                    ? a.SegregatorException.SegrCase : (byte)0;
                                s2 = b.SegregatorException.SegrClass == "1" && a.SegregatorClass.StartsWith("1")
                                    ? b.SegregatorException.SegrCase : (byte)0;
                                _seglevel = s1 > s2 ? s1 : s2;
                            } //in relation to goods of class 1

                            else _seglevel = (byte)Segregation.GetSegregationLevelFromTable(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99)
                            {
                                _seglevel = s1 > s2
                                    ? (byte)Segregation.GetSegregationLevelFromTable(a.DgClass, b.SegregatorClass)
                                    : (byte)Segregation.GetSegregationLevelFromTable(b.DgClass, a.SegregatorClass);
                            }
                            //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                }
            }

            Debug.WriteLine("Seglevel is {0}", seglevel);
            Debug.WriteLine("Seglevel5 is {0}", _seglevel5);
            if (_seglevel5 != 0) _conf = Segregation.SegregationConflictCheck(a, b, _seglevel5);
            if (Segregation.SegregationConflictCheck(a, b, seglevel)) _conf = true;
            Debug.WriteLine("Segregation result is {0}", _conf.ToString());
            Assert.IsTrue(_conf);
        }

        [TestMethod]
        public void DefineCargoHoldNumberTest()
        {

            Debug.WriteLine("Dg Bay is: {0}", dgList[0].Bay);
            Debug.WriteLine("Hold stored in dg before defining is: {0}", dgList[0].HoldNr);
            byte hold = ShipProfile.DefineCargoHoldNumber(dgList[0].Bay);
            Debug.WriteLine("Hold answer found is: {0}", hold);

            Assert.AreEqual(2, hold);
        }
    }
}
