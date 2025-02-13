"""
Created on May 14, 2023
@author: Lance A. Endres
"""

from unicodetolatex import unicode_codes_to_latex
file = open("LaTeX Tag Quality Processing.qlty", "w")

def WriteLine(line):
    file.write(line+"\n")

WriteLine('<?xml version="1.0" encoding="utf-8"?>')
WriteLine("")
WriteLine('<!-- Do not directly edit this file.  Edit and run the Python script in the "Ancillary Files" directory to make changes. -->')
WriteLine("")
WriteLine('<tagprocessorgroup name="Unicode to LaTeX" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">')
WriteLine('\t<tagprocessors>')

for key, value in unicode_codes_to_latex.items():
    WriteLine('\t\t<tagprocessor xsi:type="StringReplacementTagProcessor"')
    WriteLine('\t\t\ttagstoprocess="ExcludeSpecified"')
    WriteLine('\t\t\tpattern="' + '\\u' + key + '"')
    WriteLine('\t\t\treplacement="' + value.replace('"', '&quot;') + '">')
    WriteLine('\t\t\t<tags>')
    WriteLine('\t\t\t\t<tag>href</tag>')
    WriteLine('\t\t\t\t<tag>url</tag>')
    WriteLine('\t\t\t\t<tag>doi</tag>')
    WriteLine('\t\t\t</tags>')
    WriteLine('\t\t</tagprocessor>')

WriteLine('\t</tagprocessors>')
WriteLine('</tagprocessorgroup>')
file.close()