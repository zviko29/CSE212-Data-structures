using Microsoft.VisualStudio.TestTools.UnitTesting;

// Problem 2 - Priority Queue test cases

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue three items with different priorities (Low:1, High:5, Medium:3),
    // then dequeue three times.
    // Expected Result: High, Medium, Low (highest priority dequeued first each time)
    // Defect(s) Found: The Dequeue loop stopped one element early (index < _queue.Count - 1),
    // so the highest priority item was sometimes never even considered.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("High", 5);
        priorityQueue.Enqueue("Medium", 3);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Medium", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue the highest-priority item last in the list (Low:1, Medium:3, High:10),
    // then dequeue once.
    // Expected Result: High
    // Defect(s) Found: Because the loop used index < _queue.Count - 1, the last item in the
    // list (index Count-1) was never checked, so a highest-priority item added last was
    // skipped entirely.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("Medium", 3);
        priorityQueue.Enqueue("High", 10);

        Assert.AreEqual("High", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue two items with the SAME priority (A:1, B:1), then dequeue twice.
    // Expected Result: A, B (ties broken by FIFO order - the one closest to the front goes first)
    // Defect(s) Found: Two defects - (1) the tie-breaking comparison used >= instead of >,
    // so the LAST item with a tied priority was chosen instead of the first (violates FIFO).
    // (2) Dequeue never removed the item from _queue (missing RemoveAt), so the same item
    // would be returned again and the queue would never shrink.
    public void TestPriorityQueue_3()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 1);

        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Call Dequeue on a brand-new, empty PriorityQueue.
    // Expected Result: An InvalidOperationException is thrown with the message
    // "The queue is empty."
    // Defect(s) Found: None - this case was already implemented correctly.
    public void TestPriorityQueue_4()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                string.Format("Unexpected exception of type {0} caught: {1}",
                               e.GetType(), e.Message)
            );
        }
    }

    [TestMethod]
    // Scenario: Enqueue several items, dequeue all of them one at a time, and verify the
    // queue drains completely in strict priority order (with a tie broken by FIFO along the way).
    // Expected Result: D(9), A(5), C(5), B(2) - and the queue is empty afterward (Dequeue on
    // the empty queue throws).
    // Defect(s) Found: Same missing RemoveAt defect as above - without removal, this full
    // drain scenario would loop forever returning the same top item, or would return
    // already-removed items again.
    public void TestPriorityQueue_5()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 5);
        priorityQueue.Enqueue("B", 2);
        priorityQueue.Enqueue("C", 5);
        priorityQueue.Enqueue("D", 9);

        Assert.AreEqual("D", priorityQueue.Dequeue());
        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());

        Assert.ThrowsException<InvalidOperationException>(() => priorityQueue.Dequeue());
    }
}