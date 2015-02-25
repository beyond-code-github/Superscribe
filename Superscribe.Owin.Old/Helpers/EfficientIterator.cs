using System;
using System.Collections.Generic;

namespace Superscribe.Owin.Helpers
{
    public class EfficientIterator<TOne, TTwo>
    {
        private Action<TOne, TTwo> always = (one, two) => { };
        private Action<TOne, TTwo> match = (one, two) => { };
        private Action<TOne> oneOnly = (one) => { };
        private Action<TTwo> twoOnly = (two) => { };

        private readonly Func<TOne, TTwo, int> compare;

        public Action<TOne, TTwo> Always { set { this.always = value; } }
        public Action<TOne, TTwo> Match { set { this.match = value; } }
        public Action<TOne> OneOnly { set { this.oneOnly = value; } }
        public Action<TTwo> TwoOnly { set { this.twoOnly = value; } }

        public EfficientIterator(Func<TOne, TTwo, int> compare)
        {
            this.compare = compare;
        }

        public void RunToEnd(IEnumerable<TOne> listOne, IEnumerable<TTwo> listTwo)
        {
            using (var listOneEnumerator = listOne.GetEnumerator())
            using (var listTwoEnumerator = listTwo.GetEnumerator())
            {
                var listOneIterating = listOneEnumerator.MoveNext();
                var listTwoIterating = listTwoEnumerator.MoveNext();

                while (listOneIterating || listTwoIterating)
                {
                    int result = 0;
                    if (listOneIterating && listTwoIterating)
                    {
                        result = this.compare(listOneEnumerator.Current, listTwoEnumerator.Current);

                        // a and b are equal
                        if (result == 0)
                        {
                            this.always(listOneEnumerator.Current, listTwoEnumerator.Current);
                            this.match(listOneEnumerator.Current, listTwoEnumerator.Current);

                            listOneIterating = listOneEnumerator.MoveNext();
                            listTwoIterating = listTwoEnumerator.MoveNext();

                            continue;
                        }
                    }

                    // list one has run out, or it's current item is greater than list two
                    if (!listOneIterating || result > 0)
                    {
                        this.always(default(TOne), listTwoEnumerator.Current);
                        this.twoOnly(listTwoEnumerator.Current);

                        listTwoIterating = listTwoEnumerator.MoveNext();
                        continue;
                    }

                    // list two has run out, or it's current item is greater than list one
                    if (!listTwoIterating || result < 0)
                    {
                        this.always(listOneEnumerator.Current, default(TTwo));
                        this.oneOnly(listOneEnumerator.Current);

                        listOneIterating = listOneEnumerator.MoveNext();
                        continue;
                    }
                }
            }
        }
    }
}
