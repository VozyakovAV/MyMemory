using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyMemory.Tests
{
    [TestClass]
    public class ParseWords : BaseTest
    {
        [TestMethod]
        public void ParseWordsStart()
        {
            /*var ctx = new MemoryDbContext();
            MemoryDBInitializer.AddGroup(ctx);*/
        }

        [TestMethod]
        public void ParseItems()
        {
            var ctx = new MemoryDbContext();

            var dir = new DirectoryInfo(@"C:\temp\english");
            var files = dir.GetFiles();

            var group = ctx.Groups.First(x => x.Name == "Английский");

            var group2 = new MemoryGroup(Path.GetFileNameWithoutExtension("16 уроков Петрова"), group);
            ctx.Groups.Add(group2);

            foreach (var file in files)
            {
                AddItems(ctx, group2, file.FullName);
            }

            ctx.SaveChanges();
        }

        private void AddItems(MemoryDbContext ctx, MemoryGroup group, string fileName)
        {
            var text = File.ReadAllText(fileName, Encoding.GetEncoding(1251));
            var listItems = ParseItems(text);

            var groupSub = new MemoryGroup(Path.GetFileNameWithoutExtension(fileName), group);
            ctx.Groups.Add(groupSub);

            listItems.ForEach(x => x.Group = groupSub);
            ctx.Items.AddRange(listItems);
        }

        private List<MemoryItem> ParseItems(string text)
        {
            var listItems = new List<MemoryItem>();
            var blocks = Regex.Split(text, "={4}=+");

            foreach (var block in blocks)
            {
                var lines = Regex.Split(block, @"-{4}-+");
                if (lines.Length == 2)
                {
                    var item = new MemoryItem()
                    {
                        Question = lines[0].Trim(),
                        Answer = lines[1].Trim()
                    };
                    listItems.Add(item);
                }
                else
                {

                }
            }
            return listItems;
        }
    }
}
