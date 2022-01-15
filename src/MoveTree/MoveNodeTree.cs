using System;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNodeTree
    {
        //The move nodes
        private MoveNode RootNode; //This is just a place holder that is used to organize the first moves that can be made by white. The child move nodes of this move node are white's potential first moves. This is not surfaced to the user.
        public MoveNode[] MoveNodes
        {
            get
            {
                return RootNode.ChildNodes;
            }
        }
    
        //Adding a new first move
        public void AddFirstMoveNode(MoveNode node)
        {
            try
            {
                RootNode.AddChildNode(node);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to add first move node. Msg: " + ex.Message);
            }
        }

    }

}