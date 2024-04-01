﻿using System.Collections.Generic;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO;

public class ExamDAO
{
    private static ExamDAO? instance;
    private Dictionary<string, Exam>? exams;
    private readonly LastIdDAO lastIdDao = LastIdDAO.GetInstance();

    private Dictionary<string, Exam> Exams
    {
        get
        {
            exams ??= JsonUtil.ReadFromFile<Exam>(Constants.ExamFilePath);
            return exams!;
        }
        set => exams = value;
    }

    private ExamDAO()
    {
    }

    public static ExamDAO GetInstance()
    {
        return instance ??= new ExamDAO();
    }

    public Dictionary<string, Exam> GetAllExams()
    {
        return Exams;
    }

    public Exam? GetExamById(string id)
    {
        return Exams.GetValueOrDefault(id);
    }
    
    public List<Exam> GetExamsForIds(List<string> ids)
    {
        List<Exam> exams = new();
        foreach (string id in ids)
        {
            if (Exams.ContainsKey(id))
            {
                exams.Add(Exams[id]);
            }
        }
        return exams;
    }

    public Exam AddExam(Exam exam)
    {
        lastIdDao.IncrementExamId();
        exam.Id = lastIdDao.GetExamId();
        Exams.Add(exam.Id, exam);
        SaveExams();
        return exam;
    }

    public Exam? UpdateExam(string id, Exam exam)
    {
        if (!Exams.ContainsKey(id)) return null;
        Exams[id] = exam;
        SaveExams();
        return exam;
    }

    public void DeleteExam(string id)
    {
        Exams.Remove(id);
        SaveExams();
    }

    private void SaveExams()
    {
        JsonUtil.WriteToFile(Exams, Constants.ExamFilePath);
    }
}