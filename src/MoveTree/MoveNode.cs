using System;
using System.Collections.Generic;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNode
    {
        public string Move {get; set;}
        public uint Occurances {get; set;}
        public MoveNode[] ChildNodes {get; set;}

        #region "Constructors"

        private void Initialize()
        {
            ChildNodes = new MoveNode[]{};
        }

        public MoveNode()
        {
            Initialize();
        }

        public MoveNode(string move)
        {
            Initialize();
            Move = move;
        }

        #endregion

        public void AddChildNode(MoveNode node)
        {
            //First ensure that a child node with this move does not already exist
            foreach (MoveNode n in ChildNodes)
            {
                if (n.Move == node.Move)
                {
                    throw new Exception("A node for move '" + node.Move + "' alraedy exists in this parent node.");
                }
            }

            //Add it
            List<MoveNode> NewFullList = new List<MoveNode>();
            NewFullList.AddRange(ChildNodes);
            NewFullList.Add(node);
            ChildNodes = NewFullList.ToArray();
        }

        public MoveNode FindChildNode(string move)
        {
            foreach (MoveNode node in ChildNodes)
            {
                if (node.Move == move)
                {
                    return node;
                }
            }
            return null;
        }

    }
}