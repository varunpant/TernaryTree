namespace TernarySearchTree
{
    using System;
    using System.Text;

    /// <summary>
    /// Straightforward implementation of the Porter stemming algorithm described in:
    /// Program, Vol 14 no. 3 pp 130-137, July 1980.
    /// </summary>
    /// <remarks>
    /// The perf of this class can be optimized a lot if needed.
    /// </remarks>
    public class Stemmer
    {
        private StringBuilder _buffer;

        public string Stem(string word)
        {
            return Stem(word, 3);
        }

        public string Stem(string word, int minLength)
        {
            word = word.ToLower();
            if (word.Length < minLength)
            {
                return word;
            }

            _buffer = new StringBuilder(word);

            DoStep1();
            DoStep2();
            DoStep3();
            DoStep4();
            DoStep5();

            return _buffer.ToString();
        }

        private bool IsConsonant(int idx)
        {
            char c = _buffer[idx];
            return c != 'a' && c != 'e' && c != 'i' && c != 'o' && c != 'u' &&
                (c != 'y' || idx == 0 || !IsConsonant(idx - 1));
        }

        private int Measure(int endLength)
        {
            int i = 0, endIdx = _buffer.Length - endLength;
            while (true)
            {
                if (i == endIdx)
                {
                    return 0;
                }

                if (!IsConsonant(i))
                {
                    break;
                }

                i++;
            }

            int m = 0;
            while (true)
            {
                while (true)
                {
                    if (i == endIdx)
                    {
                        return m;
                    }

                    if (IsConsonant(i))
                    {
                        break;
                    }

                    i++;
                }

                m++;

                while (true)
                {
                    if (i == endIdx)
                    {
                        return m;
                    }

                    if (!IsConsonant(i))
                    {
                        break;
                    }

                    i++;
                }
            }
        }

        private bool EndsWith(string s)
        {
            if (_buffer.Length < s.Length)
            {
                return false;
            }

            int startIdx = _buffer.Length - s.Length;
            int endIdx = startIdx + s.Length;

            for (int i = startIdx; i < endIdx; i++)
            {
                if (_buffer[i] != s[i - startIdx])
                {
                    return false;
                }
            }

            return true;
        }

        private bool ContainsVowel(int endLength)
        {
            for (int i = 0; i < _buffer.Length - endLength; i++)
            {
                if (!IsConsonant(i))
                {
                    return true;
                }
            }

            return false;
        }

        private bool DoubleConsonant(int endLength)
        {
            int endIdx = _buffer.Length - endLength - 1;
            if (endIdx < 1)
            {
                return false;
            }

            return _buffer[endIdx] == _buffer[endIdx - 1] && IsConsonant(endIdx);
        }

        private bool EndsWithCVC(int endLength)
        {
            int endIdx = _buffer.Length - endLength - 1;
            if (endIdx < 2)
            {
                return false;
            }

            return IsConsonant(endIdx - 2) && !IsConsonant(endIdx - 1) && IsConsonant(endIdx)
                && _buffer[endIdx] != 'w' && _buffer[endIdx] != 'x' && _buffer[endIdx] != 'y';
        }

        private void DoStep1()
        {
            if (EndsWith("sses"))
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("ies"))
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("s") && !EndsWith("ss"))
            {
                _buffer.Length--;
            }

            if (EndsWith("eed"))
            {
                if (Measure(3) > 0)
                {
                    _buffer.Length--;
                }
            }
            else if (EndsWith("ed"))
            {
                if (ContainsVowel(2))
                {
                    _buffer.Length -= 2;
                    DoStep1Helper();
                }
            }
            else if (EndsWith("ing"))
            {
                if (ContainsVowel(3))
                {
                    _buffer.Length -= 3;
                    DoStep1Helper();
                }
            }

            if (EndsWith("y") && ContainsVowel(1))
            {
                _buffer[_buffer.Length - 1] = 'i';
            }
        }

        private void DoStep1Helper()
        {
            if (EndsWith("at"))
            {
                _buffer.Append('e');
            }
            else if (EndsWith("bl"))
            {
                _buffer.Append('e');
            }
            else if (EndsWith("iz"))
            {
                _buffer.Append('e');
            }
            else if (DoubleConsonant(0) && !EndsWith("l") && !EndsWith("s") && !EndsWith("z"))
            {
                _buffer.Length--;
            }
            else if (Measure(0) == 1 && EndsWithCVC(0))
            {
                _buffer.Append('e');
            }
        }

        private void DoStep2()
        {
            if (EndsWith("ational") && Measure(7) > 0)
            {
                _buffer.Length -= 4;
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("tional") && Measure(6) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("enci") && Measure(4) > 0)
            {
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("anci") && Measure(4) > 0)
            {
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("izer") && Measure(4) > 0)
            {
                _buffer.Length--;
            }
            else if (EndsWith("abli") && Measure(4) > 0)
            {
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("alli") && Measure(4) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("entli") && Measure(5) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("eli") && Measure(3) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("ousli") && Measure(5) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("ization") && Measure(7) > 0)
            {
                _buffer.Length -= 4;
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("ation") && Measure(5) > 0)
            {
                _buffer.Length -= 2;
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("ator") && Measure(4) > 0)
            {
                _buffer.Length--;
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("alism") && Measure(5) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("iveness") && Measure(7) > 0)
            {
                _buffer.Length -= 4;
            }
            else if (EndsWith("fulness") && Measure(7) > 0)
            {
                _buffer.Length -= 4;
            }
            else if (EndsWith("ousness") && Measure(7) > 0)
            {
                _buffer.Length -= 4;
            }
            else if (EndsWith("aliti") && Measure(5) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("iviti") && Measure(5) > 0)
            {
                _buffer.Length -= 2;
                _buffer[_buffer.Length - 1] = 'e';
            }
            else if (EndsWith("biliti") && Measure(6) > 0)
            {
                _buffer.Length -= 3;
                _buffer[_buffer.Length - 2] = 'l';
                _buffer[_buffer.Length - 1] = 'e';
            }
        }

        private void DoStep3()
        {
            if (EndsWith("icate") && Measure(5) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("ative") && Measure(5) > 0)
            {
                _buffer.Length -= 5;
            }
            else if (EndsWith("alize") && Measure(5) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("iciti") && Measure(5) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("ical") && Measure(4) > 0)
            {
                _buffer.Length -= 2;
            }
            else if (EndsWith("ful") && Measure(3) > 0)
            {
                _buffer.Length -= 3;
            }
            else if (EndsWith("ness") && Measure(4) > 0)
            {
                _buffer.Length -= 4;
            }
        }

        private void DoStep4()
        {
            if (EndsWith("al"))
            {
                if (Measure(2) > 1)
                {
                    _buffer.Length -= 2;
                }
            }
            else if (EndsWith("ance"))
            {
                if (Measure(4) > 1)
                {
                    _buffer.Length -= 4;
                }
            }
            else if (EndsWith("ence"))
            {
                if (Measure(4) > 1)
                {
                    _buffer.Length -= 4;
                }
            }
            else if (EndsWith("er"))
            {
                if (Measure(2) > 1)
                {
                    _buffer.Length -= 2;
                }
            }
            else if (EndsWith("ic"))
            {
                if (Measure(2) > 1)
                {
                    _buffer.Length -= 2;
                }
            }
            else if (EndsWith("able"))
            {
                if (Measure(4) > 1)
                {
                    _buffer.Length -= 4;
                }
            }
            else if (EndsWith("ible"))
            {
                if (Measure(4) > 1)
                {
                    _buffer.Length -= 4;
                }
            }
            else if (EndsWith("ant"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ement"))
            {
                if (Measure(5) > 1)
                {
                    _buffer.Length -= 5;
                }
            }
            else if (EndsWith("ment"))
            {
                if (Measure(4) > 1)
                {
                    _buffer.Length -= 4;
                }
            }
            else if (EndsWith("ent"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ion"))
            {
                if (Measure(3) > 1 && (EndsWith("sion") || EndsWith("tion")))
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ou"))
            {
                if (Measure(2) > 1)
                {
                    _buffer.Length -= 2;
                }
            }
            else if (EndsWith("ism"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ate"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("iti"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ous"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ive"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
            else if (EndsWith("ize"))
            {
                if (Measure(3) > 1)
                {
                    _buffer.Length -= 3;
                }
            }
        }

        private void DoStep5()
        {
            if (EndsWith("e"))
            {
                int m = Measure(1);
                if (m > 1 || (m == 1 && !EndsWithCVC(1)))
                {
                    _buffer.Length--;
                }
            }

            if (EndsWith("l") && Measure(1) > 1 && DoubleConsonant(0))
            {
                _buffer.Length--;
            }
        }
    }
}
