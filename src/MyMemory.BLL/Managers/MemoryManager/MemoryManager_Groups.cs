﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryGroup FindGroup(int groupId)
        {
            return _groupRepository.GetItems().FirstOrDefault(x => x.Id == groupId);
        }

        public MemoryGroup[] GetGroups()
        {
            return _groupRepository.GetItems().ToArray();
        }

        public MemoryGroup GetGroup(int groupId)
        {
            return _groupRepository.GetItems()
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == groupId);
        }

        public int[] GetTreeId(int groupId)
        {
            var groups = _groupRepository.GetItems()
                .Select(x => new ParentChild() { Id = x.Id, ParentId = x.Parent.Id })
                .ToList();

            var list = new List<int>();
            if (groups.Any(x => x.Id == groupId))
            {
                list.Add(groupId);
            }

            if (groupId == 0)
            {
                list.AddRange(groups.Select(x => x.Id));
            }
            else
            {
                list.AddRange(SelectChilds(groupId, groups));
            }

            return list.ToArray();
        }

        public List<MemoryGroup> GetTreeGroups(int? groupID = null)
        {
            var res = _groupRepository.GetItems()
                .Include("Childs.Childs.Childs.Childs.Childs.Childs.Childs");

            if (groupID == null)
            {
                res = res.Where(x => x.Parent == null);
            }
            else
            {
                res = res.Where(x => x.Parent != null && x.Parent.Id == groupID);
            }

            return res.ToList();
        }

        public MemoryGroup SaveGroup(MemoryGroup group)
        {
            var group2 = _groupRepository.Save(group);
            _uow.Commit();
            return group2;
        }

        public void DeleteGroup(MemoryGroup group)
        {
            _groupRepository.Delete(group);
            _uow.Commit();
        }

        // -------------------------

        private List<int> SelectChilds(int parentId, List<ParentChild> list)
        {
            var result = new List<int>();
            var childs = list.Where(x => x.ParentId == parentId).ToList();

            foreach (var child in childs)
                result.Add(child.Id);

            foreach (var child in childs)
                result.AddRange(SelectChilds(child.Id, list));

            return result;
        }

        private class ParentChild
        {
            public int Id { get; set; }
            public int? ParentId { get; set; }
        }
    }
}
