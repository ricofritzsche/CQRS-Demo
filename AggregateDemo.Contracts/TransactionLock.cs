namespace AggregateDemo.Common
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Transactions;

    /// <summary>
    /// Die Transaction Lock Klasse, die die Sperrung von Objekten, die sich momentan in einer Transaktion befinden ermöglicht.
    /// </summary>
    public sealed class TransactionLock
    {
        private static readonly object LockObject = new object();

        private readonly LinkedList<KeyValuePair<Transaction, ManualResetEvent>> pendingTransactions =
            new LinkedList<KeyValuePair<Transaction, ManualResetEvent>>();

        private Transaction owningTransaction;

        public bool Locked
        {
            get { return this.owningTransaction != null; }
        }

        public void Lock()
        {
            if (Transaction.Current != null)
            {
                this.Lock(Transaction.Current);
            }
        }

        private void Lock(Transaction transaction)
        {
            Monitor.Enter(this);

            if (this.owningTransaction == null)
            {
                if (transaction != null)
                {
                    this.owningTransaction = transaction;
                }

                Monitor.Exit(this);
            }
            else
            {
                if (this.owningTransaction == transaction)
                {
                    Monitor.Exit(this);
                }
                else
                {
                    var manualEvent = new ManualResetEvent(false);
                    var pair = new KeyValuePair<Transaction, ManualResetEvent>(transaction, manualEvent);
                    this.pendingTransactions.AddLast(pair);

                    if (transaction != null)
                    {
                        transaction.TransactionCompleted += delegate
                        {
                            lock (this)
                            {
                                this.pendingTransactions.Remove(pair);
                            }

                            lock (LockObject)
                            {
                                if (!manualEvent.SafeWaitHandle.IsClosed)
                                {
                                    manualEvent.Set();
                                }
                            }
                        };
                    }

                    Monitor.Exit(this);

                    manualEvent.WaitOne();
                    lock (LockObject)
                    {
                        manualEvent.Close();
                    }
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public void Unlock()
        {
            lock (this)
            {
                this.owningTransaction = null;
                LinkedListNode<KeyValuePair<Transaction, ManualResetEvent>> node = null;
                if (this.pendingTransactions.Count > 0)
                {
                    node = this.pendingTransactions.First;
                    this.pendingTransactions.RemoveFirst();
                }

                if (node != null)
                {
                    var transaction = node.Value.Key;
                    var manualEvent = node.Value.Value;

                    this.Lock(transaction);
                    lock (LockObject)
                    {
                        if (!manualEvent.SafeWaitHandle.IsClosed)
                        {
                            manualEvent.Set();
                        }
                    }
                }
            }
        }
    }
}