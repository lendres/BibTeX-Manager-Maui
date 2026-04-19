# About
BibTex Manager is a bib file editor built for individuals who write LaTeX markup (i.e., they don't use WYSIWYG style editors).  It is focused on rapid entry, fixing, and organization of bibliography entries.  It is built for speed and ease of entry.  It is designed for the customization of tools for fixing entries.  It may take a bit of customization to achieve all the results you want.  However, once done, you can automatically correct issues and create clean BibTeX entries with a couple of clicks.

 It has a heavy emphasis on two features:
* Importing and correcting bibliography entries from third-party sources.
* Organizing the structure and order of bibliography entries and files.

## Importing
Third-party bibliography entries are often structured in odd or inconsistent ways.  They also frequently contain errors and do not use meaningful names for the cite keys.  _BibTeX Manager_ includes error fixing features such as:
- Converting standard text to LaTeX (e.g., converting "&" to "\&").
- Convert quotation marks ("...") to LaTeX quotation marks (``...'').
- Convert unicode characters to LaTeX (e.g., "&AElig;" to "\AE").
- Renaming tag names.
- Automatically generating meaningful cite keys.
- Adding brackets around items that should remain capitalized (e.g., converting "3D" to {3D}).
- Preserving sentence-ending spacing after a capitalized abbreviation (e.g., converting "NASA." to "NASA\@.").
- Convert _ALL CAPS_ to _Title Case_.
- And many more.

This behavior is part of a sophisticated and generalized system.  It is completely customizable, allowing the user to change or remove existing corrections and create their own.

## Organization
_BibTeX Manager_ includes features to keep bibliography entries and files ordered.  Think of it as your personal OCD relief assistant.

# What _BibTex Manager_ Is Not
It is not a generalized full-feature reference/research manager.  If you are looking for one of those, consider Jabref.  It is a full-featured manager that includes algorithms for common cleanup issues.
https://www.jabref.org/
