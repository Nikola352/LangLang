﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LangLang.Domain.Model
{
    public class Tutor : Person, IEntity
    {
        public string Id { get; set; }
        
        /// <summary> Hold counts for 1 to 10 rating. </summary>
        public int[] RatingCounts { get; set; } = new int[10];

        public List<Tuple<Language, LanguageLevel>> KnownLanguages { get; set; }

        public DateTime DateAdded { get; set; }
        
        public Tutor() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            KnownLanguages = new();
            DateAdded = DateTime.Now;
        }
		
        public Tutor(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            KnownLanguages = knownLanguages;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }
        public Tutor(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            KnownLanguages = knownLanguages;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }

        public string GetFullName()
        {
            return Name + " " + Surname;
        }

        public double GetAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < RatingCounts.Length; i++)
            {
                sum += (i+1) * RatingCounts[i];
            }
            return (double)sum / RatingCounts.Length;
        }

        public void AddRating(int rating)
        {
            rating--;
            if (rating < 0 || rating > RatingCounts.Length)
                throw new ArgumentOutOfRangeException($"Rating is too " + ((rating < 0) ? "low" : "high"));
            else
                RatingCounts[rating]++;
        }

        public bool KnowsLanguage(Language language, LanguageLevel languageLevel)
        {
            foreach (var knownLanguage in KnownLanguages)
            {
                if(!Equals(knownLanguage.Item1, language))
                    continue;
                if (knownLanguage.Item2 < languageLevel)
                    return false;
                return true;
            }
            return false;
        }

        public double GetScore(Language language, LanguageLevel languageLevel)
        {
            var knownLanguageLevelIdx = -1;
            for (int i = 0; i < KnownLanguages.Count; i++)
            {
                if (Equals(KnownLanguages[i].Item1, language))
                {
                    knownLanguageLevelIdx = i;
                    break;
                }
            }

            int languageLevelScore = knownLanguageLevelIdx == -1
                ? -(int)languageLevel
                : KnownLanguages[knownLanguageLevelIdx].Item2 - languageLevel;
            
            return GetAverageRating() == 0 ? 1000 : 0
                + 10 * languageLevelScore
                + 100 * GetAverageRating()
                ;
        }
    }
}