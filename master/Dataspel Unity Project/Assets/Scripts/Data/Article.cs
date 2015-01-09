using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Article {
    public List<Author> Authors
    {
        get { return this._authors; }
        private set { this._authors = value; }
    } private List<Author> _authors;

    public List<University> Universities
    {
        get { return this._universities; }
        private set { this._universities = value; }
    } private List<University> _universities;

    public string Title
    {
        get { return this._title; }
        private set { this._title = value; }
    }private string _title;

    public string ArticleAbstract
    {
        get { return this._articleAbstract; }
        private set { this._articleAbstract = value; }
    } private string _articleAbstract;

    public Article(string title, string articleAbstract, Author author, University university)
    {
        this.Title = title;
        this.ArticleAbstract = articleAbstract;
        this.Authors = new List<Author>();
        this.Authors.Add(author);
        this.Universities = new List<University>();
        this.Universities.Add(university);
    }

    public Article(string title, string articleAbstract, List<Author> authors, List<University> universities)
    {
        this.Title = title;
        this.ArticleAbstract = articleAbstract;
        this.Authors = authors;
        this.Universities = universities;
    }

}
