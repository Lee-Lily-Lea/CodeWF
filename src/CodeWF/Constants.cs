﻿namespace CodeWF;

class Url
{
    public static string GetBbsPostUrl(BlogPost post) => $"./bbs/post/{post.Date?.Year}/{post.Date?.Month}/{post.Slug}";
}