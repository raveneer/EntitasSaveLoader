excel2json
==========
* Copyright (C) 2016 by WooJun Shim woojun.shim@gmail.com
* Korean description available in http://nvidian7.github.io/development/2016/07/16/excel2json.html
* excel2json is tool for generating JSON files from well-formed excel data

## Purpose
* How to represent game data is the biggest part when you develop some game
 * Generally, designers wants to manage data in excel
 * Programmers want to dear with text-based, hierarchical structured data (e.g. JSON, XML) 
* excel2json can be easily generate JSON files from excel data 

## Features
* Auto-search source Excel files in same directory
* JSON file generated per workbook's sheet (JSON filename would be same with Excel sheet name)
    * Sheet name must be prefixed '!' mark (see example.xlsx)
    * Not-prefixed sheet will be ignored
* Supports complex hierarchy JSON model
    * Object in Objects, Array in Objects, Objects in Array all of cases that JSON can represents
    * N-depth hierarchy ( as you want to )
* Excel formula evaluatation supports
* Using cell merge feature to represents Object or Array's scope
* Pretty printed JSON output
* It will preserve the order of properties written in Excel file  

## How to define JSON scheme in xls file 

* Please see sample.xlsx and commited json files :)
* You can understand more easily If you ar familiar with JSON-structure


## Usage
Just typing this :D

```
java -jar excel2json-standalone.jar
```

## Constraints
* Supports only version of Excel 2007 (or higher)
* excel2json needs at least the Java Runtime Environment 1.8
* Source-code is not provide yet, sort of...

## Similar projects that I've found on GitHub
* https://github.com/coolengineer/excel2json
* https://github.com/mhaemmerle/excel-to-json

## Command line options
* --generate-hash
    * Generates md5 hash json file which named 'resource_hash.json'

## Changelist

* 2016-10-18
    * Fix MD5 hash dump feature bug.

## Warning
1. It would cause poor performance if you have too many VLOOKUP formula 
2. Empty cell will be ignored
3. Not supports all of formulas. To find which formula excel2json supports read this page first (https://poi.apache.org/spreadsheet/formula.html) because excel2json implements on top of Apache POI projects. 
