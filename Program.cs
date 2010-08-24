using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<TestNode> nodeList = new List<TestNode>();

            nodeList.Add(new TestNode("abcd-123456"));
            //nodeList.Add(new TestNode("abcd-123478"));
            nodeList.Add(new TestNode("abcd-123490"));
            nodeList.Add(new TestNode("abcd-123401"));

            INodeLocator<TestNode> locator = new NodeLocator<TestNode>();

            locator.Initialize(nodeList);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(locator.Locate("abc" + i).Indentity);
            }

            

            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(rnd.Next(100000, 9999999));
            }

            Console.WriteLine(BitConverter.ToInt32(new FNV1a().ComputeHash(System.Text.Encoding.ASCII.GetBytes(sb.ToString())),0));
            //Console.WriteLine(BitConverter.ToInt32(new FNV1a().ComputeHash(System.Text.Encoding.ASCII.GetBytes(sb.ToString())), 4));

            Console.ReadLine();
        }
    }


    public class TestNode : INodeIndentity
    {

        private string indentity;

        public TestNode(string indentity)
        {
            this.indentity = indentity;
        }

        #region INodeIndentity 成员

        public object Indentity
        {
            get { return indentity; }
        }

        #endregion
    }
}
