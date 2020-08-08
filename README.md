# jaytwo.DisappearingFiles

<p align="center">
  <a href="https://jenkins.jaytwo.com/job/jaytwo.DisappearingFiles/job/master/" alt="Build Status (master)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=jaytwo.DisappearingFiles%2Fmaster&subject=build%20(master)" /></a>
  <a href="https://jenkins.jaytwo.com/job/jaytwo.DisappearingFiles/job/develop/" alt="Build Status (develop)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=jaytwo.DisappearingFiles%2Fdevelop&subject=build%20(develop)" /></a>
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/jaytwo.DisappearingFiles/" alt="NuGet Package jaytwo.DisappearingFiles">
    <img src="https://img.shields.io/nuget/v/jaytwo.DisappearingFiles.svg?logo=nuget&label=jaytwo.DisappearingFiles" /></a>
  <a href="https://www.nuget.org/packages/jaytwo.DisappearingFiles/" alt="NuGet Package jaytwo.DisappearingFiles (beta)">
    <img src="https://img.shields.io/nuget/vpre/jaytwo.DisappearingFiles.svg?logo=nuget&label=jaytwo.DisappearingFiles" /></a>
</p>

Provides a scoped way to deal with ephemeral filesystem resources: `DisappearingFileStream` and `DisappearingDirectory`.  Both
delete themselves on dispose (Or at least they try really hard to, see [jaytwo.SuperDelete](https://github.com/jakegough/jaytwo.superDelete/)).

### `DisappearingFileStream`:

The use case I created the `DisappearingFileStream` for is to stream a temp file from a REST api that will not leave behind 
orphan temp files on the filesystem.  Returning the files as byte arrays worked for a while, until the files got large
enough that it caused a real impact on server resources (like when running in containers in which you're trying to keep
the memory footprint as small as possible).

### `DisappearingDirectory`:

Sometimes you need temp-temp directories.  That is, a temp directory that is itself short-lived.  For example, sometimes you
are working with a utility that will convert a file.  You provide the input file name as a parameter, and the output files are
generated for you in the working directory.  Then, to find the files that were generated, you have to peer into the contents of
the directory.  This is troublesome if you use the system temp directory for this and your system temp directory already has 
other temp files inside.

## Installation

Add the NuGet package

```
PM> Install-Package jaytwo.DisappearingFiles
```

## Usage

TODO

---

Made with &hearts; by Jake
