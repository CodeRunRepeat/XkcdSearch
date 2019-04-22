I've always loved [xkcd](https://xkcd.com) and some of their comics really talk to me. They've been around for ages though,
and it's impossible by now to find this old comic that's so appropriate for the occasion, since I only remember the punchline
or a couple of words from the captions.

To solve this problem for myself and anyone who shares it, here is a simple, not particularly well built, Azure based solution
that OCRs the comics and provides with a search service on top of them. Captions and titles are included for the majority of
comics (except for those that had a big or complex enough image for the Computer Vision service to choke on).

Here's what it looks like. The search app is red since it's not implemented yet, but everything else should be more or less in
place. This is not a single click deployment, but I'll get around to it sooner or later. Contributions and issue reports welcome.

![Solution architecture](https://github.com/CodeRunRepeat/XkcdSearch/blob/master/docs/Architecture.png)
