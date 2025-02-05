﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface ISubCategory : ICategory
    {
        ICategory Parent { get; }
    }
}