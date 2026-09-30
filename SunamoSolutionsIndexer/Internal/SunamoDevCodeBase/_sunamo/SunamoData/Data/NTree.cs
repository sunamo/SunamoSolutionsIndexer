namespace SunamoDevCode._sunamo.SunamoData.Data;

// Generic N-ary tree data structure
// Another big popular tree implementation is on https://www.codeproject.com/Articles/12592/Generic-Tree-T-in-C
internal class NTree<T>
{
    internal T data;
    internal LinkedList<NTree<T>> children;

    internal NTree(T data)
    {
        this.data = data;
        children = new LinkedList<NTree<T>>();
    }

    internal void Traverse(NTree<T> node, Action<T> visitor)
    {
        visitor(node.data);
        foreach (NTree<T> childNode in node.children)
            Traverse(childNode, visitor);
    }
}
