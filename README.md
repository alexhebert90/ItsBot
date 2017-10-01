# ItsBot
A .NET library for a reddit grammar bot.

ItsBot is a hobby project of mine that will comprise of the following pieces:

* A console application capable of pulling recent comments from Reddit that meet my criteria and arranging them into a format to be exported to a private API.
* A private API that will accept comments collected by the bot, store relevant data in a SQL server and/or in memory, and allow custom client applications to interact with it.
* Custom client applications to interact with the API and trigger events.


The idea is that, under the assumption that some manual review will need to be run on each comment before the bot is able to respond, I will store certain comments and meta data temporarily on my own server until or if I attempt to access the data using a client app I write. The client app will allow me to manually review usages of "its" and/or "it's" and flag correct or incorrect usage, which will result in an immediate reddit comment by my bot, along with the storage of metadata about the decision in the hopes of automating the process in the future.

My goals include making the code as flexible as possible so that it would be trivial to modify its behavior if I happen to discover an interesting pivot along the way.

I also intend everything written to be .NET Standard compliant so that each individual piece may be run on any platform, if desired. All libraries will be written against .NET Standard 2.0+, while applications will be written against .Net Core 2.0+, using language features from C# 7.1+ where applicable. This is intended both to provide maximum flexbility as well as to increase my knowledge of the changes made to Core applications.
