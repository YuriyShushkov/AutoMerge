using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMerge
{
	class WordsParser
	{
		List<string> _glossary;         // Словарик
		List<List<string>> _words;      // Список слов
		List<string> _links;            // Список ссылок на слова
		List<List<string>> _output;     // Список уникальных строк
		List<string> _outpoutLinks;     // Список ссылок уникальных строк


		string _wordPattern = @"([\w\d])+"; // Шаблон для поиска слов

		/// <summary>
		/// Прочитать файл
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		/// <returns>Результат чтения файла</returns>
		public bool ReadFile(string FileName)
		{
			_glossary = new List<string>();
			_words = new List<List<string>>();
			_links = new List<string>();

			if (!File.Exists(FileName))
			{
				Console.WriteLine($"Файл {FileName} не существует");
				return false;
			}

			var reader = File.OpenText(FileName);
			string line = reader.ReadLine();
			while (line != null)
			{
				ParseLine(line);
				// Читаем следующую строку
				line = reader.ReadLine();
			}

			return true; // Разобрали файл, ошибок не нашли
		}

		/// <summary>
		/// Разобрать строку
		/// </summary>
		/// <param name="line">Строка</param>
		void ParseLine(string line)
		{
			List<string> rowWords = new List<string>();     // Строка для списка слов
			List<int> rowLinks = new List<int>();           // Строка для ссылок на словарик
			var words = Regex.Matches(line, _wordPattern);  // Формируем список слов в предложении

			for (int i = 0; i < words.Count; i++)
			{
				string word = words[i].Value;
				string norm = word.ToLower();               // Игнорируем регистр в словах

				if (!_glossary.Contains(norm))
					_glossary.Add(norm);                    // В словарике нет еще слова, добавляем

				// Добавляем слова в список слов и список ссылок
				rowWords.Add(word);
				rowLinks.Add(_glossary.IndexOf(norm));

			}

			_words.Add(rowWords);
			rowLinks.Sort();        // Отсортируем список линков
			_links.Add(string.Join("_", rowLinks));
		}

		/// <summary>
		/// Сохранить файл
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		public void SaveOutputFile(string FileName)
		{
			var writer = File.CreateText(FileName);
			for (int i = 0; i < _output.Count; i++)
			{
				writer.WriteLine(string.Join(" ", _output[i]));
			}
		}

		/// <summary>
		/// Формируем выходные данные
		/// </summary>
		public void CreateOutput()
		{
			_output = new List<List<string>>();
			_outpoutLinks = new List<string>();
			for (int i = 0; i < _links.Count; i++)
			{
				var link = _links[i];
				if (!_outpoutLinks.Contains(link))
				{
					// Еще строка не попала в выходной массив
					_outpoutLinks.Add(link);
					_output.Add(_words[i]);
				}
			}
		}
	}
}
