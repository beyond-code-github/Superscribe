---
layout: default
title:  FAQ
---

<div class="block">
    <h2 class="title-divider"><span>FAQ</span>
    <small>Superscribe Frequently Asked Questions</small>
    </h2>
   	<div class="well well-mini pull-center">
	  <em>Don't see a question you want answering? Please contact the Author using the links at the bottom of the page!</em>
	</div>
<h3 class="visible-phone">
  Q: What is Graph Based Routing?
</h3>
<p><strong>A:</strong> Graph based routing is a route parsing strategy design for use in web applications. <a href="graphbasedrouting.html">You can read the full explanation here</a></p>
<h3 class="visible-phone">
  Q: How does Superscribe implement Graph Based Routing?
</h3>
<p><strong>A:</strong> At it's core, Superscribe provides a fully compliant Graph Based Routing implementation written in C#. It also goes one step further than this, providing a DSL for creating definitions quickly and easily, as well as helpers for dealing with OWIN pipelines and Asp.Net Web Api which are not part of  Graph Based Routing itself.
</p>
<h3 class="visible-phone">
  Q: Why bother with a graph? What's wrong with a Tree?
</h3>
<p><strong>A:</strong> There's nothing wrong with a Tree, in fact the vast majority of routing cases will fit well with a Tree implementation. A Tree is still a type of graph however so these still work with no problems.
</p>
<p>What the graph enables us to do is to provide multiple entry points into routes, reusing our definitions but enabling extra features. For example:</p>
<ul>
	<li><em>/Api/Apps</em> - regular route</li>
	<li><em>/Debug/Api/Apps</em> - same route with tracing enabled</li>
</ul>
<h3 class="visible-phone">
  Q: How do I type ʅ and ʃ?
</h3>
<p><strong>A:</strong> You can enable enhanced alt-codes in Windows 7, 8<a href="http://en.wikipedia.org/wiki/Unicode_input#Hexadecimal_code_input">using the instructions here.</a> Once you've done that, you can hold Alt and type +285 for ʅ, and +283 for ʃ. Mac OS also supports typing unicode characters by four digit hexadecmial code using the Option key (replace the + with 0).
</p>
<h3 class="visible-phone">
  Q: I still don't like those characters, can't you change them?
</h3>
<p><strong>A:</strong> You can copy and paste these aliases at the top of any source code file in order to provide different names for types if you wish (although it will make the DSL a little less neat)
</p>
<pre class="prettyprint">
    using Core = Superscribe.ʃ;
    using SuperInt = Superscribe.Models.ʃInt;
    using SuperString = Superscribe.Models.ʃString;
    using SuperBool = Superscribe.Models.ʃBool;
</pre>
</div>
