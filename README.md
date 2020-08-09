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

The use case I created the `DisappearingFileStream` for is to stream a temp file from a REST API that will not leave behind 
orphan temp files on the filesystem.  Returning the files as byte arrays worked for a while, until the files got large
enough that it caused a real impact on server resources (like when running in containers in which you're trying to keep
the memory footprint as small as possible).

There is an alternative option (though platform dependent): if  your filesystem supports it, you open a `FileStream` 
with `FileOption.DeleteOnClose` option.  I'm not sure which filesystems support the option, but I know I've tried it in the 
past and it just didn't do anything.

### `DisappearingDirectory`:

Sometimes you need _temp-_temp directories.  That is, a temp directory that is itself short-lived.  For example, sometimes you
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

```
// contrived example returning a temp file that needs to be cleaned up after when the job is done
[HttpGet]
public async Task<IActionResult> GetReport([FromForm] ReportRequest request)
{
    var reportTempFileFullName = await _reportService.CreateReportFile(request.StartDate, request.EndDate);
    var disappearingFileStream = new DisappearingFileStream(reportTempFileFullName, FileMode.Open);
    return File(disappearingFileStream, "application/pdf", Path.GetFileName(reportTempFileFullName), false);
}
```

```csharp
using (var workspace = DisappearingDirectory.CreateInTempPath())
{
  // create files you don't want to keep around and don't
  //   then they all get deleted when the workspace gets disposed
}
```

> Note: If you end up using them together (e.g. use the temp workspace to generate the a temp file to stream back
        to the user), be sure to move the file of interest to a location outside of the `DisappearingDirectory`'s 
        path (like the system temp path).  That way, the open file stream won't gum up the works trying to delete
        the DisappearingDirectory.

---

Made with &hearts; by Jake
