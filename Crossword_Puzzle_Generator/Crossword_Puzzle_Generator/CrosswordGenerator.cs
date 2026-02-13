using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crossword_Puzzle_Generator
{
    public class CrosswordGenerator
    {
        private char[,] grid;
        private List<CrosswordWord> placedWords = new List<CrosswordWord>();
        private Random random = new Random();
        private int gridSize = 40;

        public class CrosswordWord
        {
            public string Word { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsHorizontal { get; set; }
            public int Number { get; set; }
        }

        public CrosswordGenerator()
        {
            grid = new char[gridSize, gridSize];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = ' ';
                }
            }
        }

        public List<CrosswordWord> GenerateCrossword(List<string> inputWords)
        {
            placedWords.Clear();
            InitializeGrid();

            if (inputWords == null || inputWords.Count == 0)
                return placedWords;

            // Подготавливаем слова
            var wordList = inputWords
                .Select(w => new {
                    Word = w.ToUpper(),
                    Length = w.Length
                })
                .OrderByDescending(w => w.Length)
                .ThenBy(x => random.Next())
                .ToList();

            int wordNumber = 1;
            int placedCount = 0;
            int maxAttempts = 1000;

            // Размещаем первое слово
            if (wordList.Count > 0)
            {
                var firstWord = wordList[0];
                bool firstIsHorizontal = random.Next(2) == 0;

                int startX, startY;
                if (firstIsHorizontal)
                {
                    startX = random.Next(0, gridSize - firstWord.Word.Length);
                    startY = random.Next(0, gridSize);
                }
                else
                {
                    startX = random.Next(0, gridSize);
                    startY = random.Next(0, gridSize - firstWord.Word.Length);
                }

                if (PlaceWord(firstWord.Word, startX, startY, firstIsHorizontal, wordNumber))
                {
                    wordNumber++;
                    placedCount++;
                }
            }

            // Основной цикл размещения слов
            for (int attempt = 0; attempt < maxAttempts && placedCount < wordList.Count; attempt++)
            {
                for (int i = 1; i < wordList.Count; i++)
                {
                    var wordData = wordList[i];

                    // Если слово уже размещено, пропускаем
                    if (placedWords.Any(w => w.Word == wordData.Word))
                        continue;

                    bool placed = false;

                    // Пробуем разместить вертикально (приоритет)
                    if (random.Next(3) < 2)
                    {
                        placed = TryPlaceWordVertically(wordData.Word, wordNumber);
                    }

                    // Если не получилось вертикально, пробуем горизонтально
                    if (!placed)
                    {
                        placed = TryPlaceWordHorizontally(wordData.Word, wordNumber);
                    }

                    if (placed)
                    {
                        wordNumber++;
                        placedCount++;
                    }
                }
            }

            CompactGrid();
            return placedWords;
        }

        public List<CrosswordWord> GenerateCrosswordAlternative(List<string> inputWords)
        {
            placedWords.Clear();
            InitializeGrid();

            if (inputWords == null || inputWords.Count == 0)
                return placedWords;

            // Создаем анонимные объекты для удобства
            var wordItems = inputWords
                .Select(w => new {
                    Word = w.ToUpper(),
                    Length = w.Length
                })
                .ToList();

            int wordNumber = 1;

            // Группируем по длине
            var wordsByLength = wordItems
                .GroupBy(w => w.Length)
                .OrderBy(g => g.Key)
                .ToList();

            // Сначала размещаем самое длинное слово горизонтально по центру
            if (wordsByLength.Count > 0)
            {
                var longestGroup = wordsByLength.Last();
                var firstWord = longestGroup.First();
                int startX = gridSize / 2 - firstWord.Word.Length / 2;
                int startY = gridSize / 2;

                PlaceWord(firstWord.Word, startX, startY, true, wordNumber);
                wordNumber++;
            }

            // Затем пробуем разместить слова вертикально (в обратном порядке групп)
            foreach (var lengthGroup in wordsByLength.AsEnumerable().Reverse())
            {
                foreach (var wordData in lengthGroup)
                {
                    if (placedWords.Any(w => w.Word == wordData.Word))
                        continue;

                    // Пробуем разместить вертикально
                    bool placed = TryPlaceVerticalFromHorizontal(wordData.Word, wordNumber);
                    if (placed)
                    {
                        wordNumber++;
                    }
                }
            }

            // Оставшиеся слова размещаем горизонтально
            foreach (var lengthGroup in wordsByLength)
            {
                foreach (var wordData in lengthGroup)
                {
                    if (placedWords.Any(w => w.Word == wordData.Word))
                        continue;

                    // Пробуем разместить горизонтально
                    bool placed = TryPlaceHorizontalFromVertical(wordData.Word, wordNumber);
                    if (placed)
                    {
                        wordNumber++;
                    }
                }
            }

            CompactGrid();
            return placedWords;
        }

        private bool TryPlaceWordHorizontally(string word, int number)
        {
            var possiblePositions = new List<Tuple<int, int, CrosswordWord>>();

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x <= gridSize - word.Length; x++)
                {
                    if (CanPlaceWordHorizontally(word, x, y))
                    {
                        CrosswordWord intersectingWord = null;
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (grid[y, x + i] != ' ')
                            {
                                intersectingWord = placedWords.FirstOrDefault(w =>
                                    (w.IsHorizontal && w.Y == y && x + i >= w.X && x + i < w.X + w.Word.Length) ||
                                    (!w.IsHorizontal && w.X == x + i && y >= w.Y && y < w.Y + w.Word.Length));
                                break;
                            }
                        }

                        if (intersectingWord != null || IsAdjacentToExistingWord(word, x, y, true))
                        {
                            possiblePositions.Add(new Tuple<int, int, CrosswordWord>(x, y, intersectingWord));
                        }
                    }
                }
            }

            if (possiblePositions.Count > 0)
            {
                var bestPosition = possiblePositions
                    .OrderByDescending(p => p.Item3 != null)
                    .ThenBy(p => random.Next())
                    .First();

                return PlaceWord(word, bestPosition.Item1, bestPosition.Item2, true, number);
            }

            return false;
        }

        private bool TryPlaceWordVertically(string word, int number)
        {
            var possiblePositions = new List<Tuple<int, int, CrosswordWord>>();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y <= gridSize - word.Length; y++)
                {
                    if (CanPlaceWordVertically(word, x, y))
                    {
                        CrosswordWord intersectingWord = null;
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (grid[y + i, x] != ' ')
                            {
                                intersectingWord = placedWords.FirstOrDefault(w =>
                                    (!w.IsHorizontal && w.X == x && y + i >= w.Y && y + i < w.Y + w.Word.Length) ||
                                    (w.IsHorizontal && w.Y == y + i && x >= w.X && x < w.X + w.Word.Length));
                                break;
                            }
                        }

                        if (intersectingWord != null || IsAdjacentToExistingWord(word, x, y, false))
                        {
                            possiblePositions.Add(new Tuple<int, int, CrosswordWord>(x, y, intersectingWord));
                        }
                    }
                }
            }

            if (possiblePositions.Count > 0)
            {
                var bestPosition = possiblePositions
                    .OrderByDescending(p => p.Item3 != null)
                    .ThenBy(p => random.Next())
                    .First();

                return PlaceWord(word, bestPosition.Item1, bestPosition.Item2, false, number);
            }

            return false;
        }

        private bool TryPlaceVerticalFromHorizontal(string word, int number)
        {
            foreach (var placedWord in placedWords.Where(w => w.IsHorizontal))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = 0; j < placedWord.Word.Length; j++)
                    {
                        if (word[i] == placedWord.Word[j])
                        {
                            int x = placedWord.X + j;
                            int y = placedWord.Y - i;

                            if (y >= 0 && y + word.Length <= gridSize && CanPlaceWordVertically(word, x, y))
                            {
                                return PlaceWord(word, x, y, false, number);
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryPlaceHorizontalFromVertical(string word, int number)
        {
            foreach (var placedWord in placedWords.Where(w => !w.IsHorizontal))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = 0; j < placedWord.Word.Length; j++)
                    {
                        if (word[i] == placedWord.Word[j])
                        {
                            int x = placedWord.X - i;
                            int y = placedWord.Y + j;

                            if (x >= 0 && x + word.Length <= gridSize && CanPlaceWordHorizontally(word, x, y))
                            {
                                return PlaceWord(word, x, y, true, number);
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool CanPlaceWordHorizontally(string word, int x, int y)
        {
            if (x < 0 || x + word.Length > gridSize || y < 0 || y >= gridSize)
                return false;

            bool hasIntersection = false;

            for (int i = 0; i < word.Length; i++)
            {
                char currentCell = grid[y, x + i];

                if (currentCell != ' ' && currentCell != word[i])
                    return false;

                if (currentCell == word[i])
                    hasIntersection = true;

                if (currentCell == ' ')
                {
                    if (y > 0 && grid[y - 1, x + i] != ' ')
                        return false;
                    if (y < gridSize - 1 && grid[y + 1, x + i] != ' ')
                        return false;
                }

                if (i == 0 && x > 0 && grid[y, x - 1] != ' ')
                    return false;
                if (i == word.Length - 1 && x + i < gridSize - 1 && grid[y, x + i + 1] != ' ')
                    return false;
            }

            return hasIntersection || placedWords.Count == 0;
        }

        private bool CanPlaceWordVertically(string word, int x, int y)
        {
            if (x < 0 || x >= gridSize || y < 0 || y + word.Length > gridSize)
                return false;

            bool hasIntersection = false;

            for (int i = 0; i < word.Length; i++)
            {
                char currentCell = grid[y + i, x];

                if (currentCell != ' ' && currentCell != word[i])
                    return false;

                if (currentCell == word[i])
                    hasIntersection = true;

                if (currentCell == ' ')
                {
                    if (x > 0 && grid[y + i, x - 1] != ' ')
                        return false;
                    if (x < gridSize - 1 && grid[y + i, x + 1] != ' ')
                        return false;
                }

                if (i == 0 && y > 0 && grid[y - 1, x] != ' ')
                    return false;
                if (i == word.Length - 1 && y + i < gridSize - 1 && grid[y + i + 1, x] != ' ')
                    return false;
            }

            return hasIntersection || placedWords.Count == 0;
        }

        private bool IsAdjacentToExistingWord(string word, int x, int y, bool isHorizontal)
        {
            if (isHorizontal)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (y > 0 && grid[y - 1, x + i] != ' ')
                        return true;
                    if (y < gridSize - 1 && grid[y + 1, x + i] != ' ')
                        return true;
                    if (i == 0 && x > 0 && grid[y, x - 1] != ' ')
                        return true;
                    if (i == word.Length - 1 && x + i < gridSize - 1 && grid[y, x + i + 1] != ' ')
                        return true;
                }
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (x > 0 && grid[y + i, x - 1] != ' ')
                        return true;
                    if (x < gridSize - 1 && grid[y + i, x + 1] != ' ')
                        return true;
                    if (i == 0 && y > 0 && grid[y - 1, x] != ' ')
                        return true;
                    if (i == word.Length - 1 && y + i < gridSize - 1 && grid[y + i + 1, x] != ' ')
                        return true;
                }
            }

            return false;
        }

        private bool PlaceWord(string word, int x, int y, bool isHorizontal, int number)
        {
            char[,] backupGrid = (char[,])grid.Clone();
            var backupWords = new List<CrosswordWord>(placedWords);

            try
            {
                if (isHorizontal)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        grid[y, x + i] = word[i];
                    }

                    placedWords.Add(new CrosswordWord
                    {
                        Word = word,
                        X = x,
                        Y = y,
                        IsHorizontal = true,
                        Number = number
                    });
                }
                else
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        grid[y + i, x] = word[i];
                    }

                    placedWords.Add(new CrosswordWord
                    {
                        Word = word,
                        X = x,
                        Y = y,
                        IsHorizontal = false,
                        Number = number
                    });
                }

                return true;
            }
            catch
            {
                grid = backupGrid;
                placedWords = backupWords;
                return false;
            }
        }

        private void CompactGrid()
        {
            int minX = gridSize, maxX = 0;
            int minY = gridSize, maxY = 0;
            bool hasLetters = false;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] != ' ')
                    {
                        hasLetters = true;
                        if (j < minX) minX = j;
                        if (j > maxX) maxX = j;
                        if (i < minY) minY = i;
                        if (i > maxY) maxY = i;
                    }
                }
            }

            if (!hasLetters) return;

            int offsetX = minX;
            int offsetY = minY;

            foreach (var word in placedWords)
            {
                word.X -= offsetX;
                word.Y -= offsetY;
            }

            char[,] newGrid = new char[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    newGrid[i, j] = ' ';
                }
            }

            foreach (var word in placedWords)
            {
                if (word.IsHorizontal)
                {
                    for (int i = 0; i < word.Word.Length; i++)
                    {
                        newGrid[word.Y, word.X + i] = word.Word[i];
                    }
                }
                else
                {
                    for (int i = 0; i < word.Word.Length; i++)
                    {
                        newGrid[word.Y + i, word.X] = word.Word[i];
                    }
                }
            }

            grid = newGrid;
        }

        public char[,] GetGrid()
        {
            return grid;
        }

        public List<CrosswordWord> GetHorizontalWords()
        {
            return placedWords.Where(w => w.IsHorizontal).OrderBy(w => w.Number).ToList();
        }

        public List<CrosswordWord> GetVerticalWords()
        {
            return placedWords.Where(w => !w.IsHorizontal).OrderBy(w => w.Number).ToList();
        }
    }
}