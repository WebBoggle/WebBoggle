using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleClient;
using Boggle;
using System.Threading;

namespace BoggleClientTest
{
    /// <summary>
    /// Unit tests for PS9 Boggle Client. Tests connection, simulates gameplay, tests invalid I.P. address, and tests termination. 
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConnectionAndGamePlay()
        {
            new testClass1().run();
        }

        [TestMethod]
        [ExpectedException(typeof(BoggleConnectionException))]
        public void TestInvalidIp()
        {
            new testclass2().run();
        }

        [TestMethod]
        public void TestTerminate()
        {
            new testclass3().run();
        }

        [TestMethod]
        public void TestServerLost()
        {
            new TestClass4().run();
        }

        public class testClass1
        {
            private String time1;
            private String p1score1;
            private String p2score1;
            bool p1ScoreSet = false;
            bool p2ScoreSet = false;

            private String p1Op;
            private String p1Letters;

            private String p2Op;
            private String p2Letters;

            private String p1Summary;
            private String p2Summary;

            private string sixteenAs = "AAAAAAAAAAAAAAAA";
            bool timeSet = false;

            public void run()
            {
                BoggleServer.BoggleServer server = new BoggleServer.BoggleServer(1, "..\\..\\customDic.txt", sixteenAs);

                BoggleConnection conn = new BoggleConnection("localhost", "p1");
                conn.GameStarted += gameStarted;
                conn.ScoreChanged += scoreChanged1;
                conn.TimeChanged += timechanged1;
                conn.GameEnded += gameEnded1;
                conn.Connect();

                BoggleConnection conn2 = new BoggleConnection("localhost", "p2");
                conn2.GameStarted += gameStarted2;
                conn2.ScoreChanged += scoreChanged2;
                conn2.GameEnded += gameEnded2;
                conn2.Connect();

                Thread.Sleep(1200);

                Assert.AreEqual("P2", p1Op);
                Assert.AreEqual("P1", p2Op);
                Assert.AreEqual(sixteenAs, p1Letters);
                Assert.AreEqual(sixteenAs, p2Letters);
                Assert.AreEqual("1 0", p1score1);
                Assert.AreEqual("0 1", p2score1);
                Thread.Sleep(1000);
                Assert.AreEqual("1 AAA 0  0  0  0", p1Summary);
                Assert.AreEqual("0  1 AAA 0  0  0", p2Summary);

                server.Close();
            }

            private void gameEnded1(BoggleConnection conn)
            {
                p1Summary = conn.Summary;
            }

            private void gameEnded2(BoggleConnection conn)
            {
                p2Summary = conn.Summary;
            }

            private void timechanged1(BoggleConnection conn)
            {
                if (!timeSet)
                {
                    time1 = conn.TimeLeft;
                    timeSet = true;
                }
            }

            private void scoreChanged1(BoggleConnection conn)
            {
                if (!p1ScoreSet)
                {
                    p1score1 = conn.Score;
                    p1ScoreSet = true;
                }
            }

            private void scoreChanged2(BoggleConnection conn)
            {
                if (!p2ScoreSet)
                {
                    p2score1 = conn.Score;
                    p2ScoreSet = true;
                }
            }

            private void gameStarted(BoggleConnection conn)
            {
                p1Op = conn.oppName;

                p1Letters = conn.BoardLetters;
                conn.SendWord("aaa");
            }

            private void gameStarted2(BoggleConnection conn2)
            {
                p2Op = conn2.oppName;
                p2Letters = conn2.BoardLetters;
            }
        } //end of testclass 1


        public class testclass2
        {
            public void run()
            {
                BoggleConnection invalid = new BoggleConnection("badIP", "player1");
                // don't have to disconnect because connection not made
            }
        }

        public class testclass3
        {
            bool p1ReceivedTERMINATED;
            bool p2ReceivedTERMINATED;
            private string BoardLetters = "AAAAAAAAAAAAAAAA";
            
            public void run()
            {
                BoggleServer.BoggleServer server = new BoggleServer.BoggleServer(1, "..\\..\\customDic.txt", BoardLetters);
                BoggleConnection P1conn = new BoggleConnection("localhost", "p1");
                P1conn.GameTerminated += p1terminated;
                P1conn.Connect();

                BoggleConnection P2conn = new BoggleConnection("localhost", "p2");
                P2conn.GameTerminated += p2terminated;
                P2conn.Connect();

                Thread.Sleep(100);
                P1conn.Disconnect();
                Thread.Sleep(1000);

                Assert.AreEqual(false, p1ReceivedTERMINATED);
                Assert.AreEqual(true, p2ReceivedTERMINATED);

                server.Close();
            }

            private void p1terminated(BoggleConnection conn)
            {
                p1ReceivedTERMINATED = conn.gameTerminated;
            }

            private void p2terminated(BoggleConnection conn)
            {
                p2ReceivedTERMINATED = conn.gameTerminated;
            }
        }

        public class TestClass4
        {
            private string BoardLetters = "AAAAAAAAAAAAAAAA";
            private bool p1Lost = false;
            private bool p2Lost = false;

            public void run()
            {
                BoggleServer.BoggleServer server = new BoggleServer.BoggleServer(3, "..\\..\\customDic.txt", BoardLetters);
                BoggleConnection conn1 = new BoggleConnection("localhost", "P1");
                BoggleConnection conn2 = new BoggleConnection("localhost", "P2");

                conn1.ServerLost += p1LostServer;
                conn2.ServerLost += p2LostServer;

                conn1.Connect();
                conn2.Connect();

                Thread.Sleep(1000);
                server.Close();

                while (!p1Lost && !p2Lost) { Thread.Sleep(10); }

                Assert.AreEqual(true, p1Lost);
                Assert.AreEqual(true, p2Lost);

                server.Close();
            }

            private void p1LostServer(BoggleConnection conn)
            {
                p1Lost = true;
            }

            private void p2LostServer(BoggleConnection conn)
            {
                p2Lost = true;
            }            
        }
    }
}
