// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "<Pending>", Scope = "namespace", Target = "~N:BibTexManager")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Primary constructor prevents CommunityToolkit generator code from functioning properly.", Scope = "member", Target = "~M:BibTexManager.ViewModels.CorrectionViewModel.#ctor(BibtexManager.TagProcessingData)")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>", Scope = "member", Target = "~M:BibTexManager.ViewModels.MainViewModel.#ctor(DigitalProduction.Maui.Services.IRecentPathsManagerService,DigitalProduction.Maui.Services.IDialogService)")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Non-static for syntax purposes (call from variable).", Scope = "member", Target = "~P:BibTexManager.ViewModels.MainViewModel.Project")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Non-static for syntax purposes (call from variable).", Scope = "member", Target = "~P:BibTexManager.ViewModels.MainViewModel.SavePathRequired")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Non-static for syntax purposes (call from variable).", Scope = "member", Target = "~M:BibTexManager.ViewModels.MainViewModel.SingleImport(BibtexManager.ISingleImporter,System.String)~BibTeXLibrary.BibEntry")]