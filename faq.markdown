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
<p><strong>A:</strong> At it's core, Superscribe provides a fully compliant Graph Based Routing implementation written in C#. It also goes one step further than this, providing a DSL for creating definitions quickly and easily, as well as helpers for dealing with Owin pipelines and Asp.Net Web Api which are not part of  Graph Based Routing itself.
</p>
<h3 class="visible-phone">
  Q: Why bother with a graph? What's wrong with a tree?
</h3>
<p><strong>A:</strong> There's nothing wrong with a Tree, in fact the vast majority of routing cases will fit well with a Tree implementation. A Tree is still a type of graph however so these still work with no problems.
</p>
<p>What the graph enables us to do is to provide multiple entry points into routes, reusing our definitions but enabling extra features. For example:</p>
<ul>
	<li><em>/Api/Apps</em> - regular route</li>
	<li><em>/Debug/Api/Apps</em> - same route with tracing enabled</li>
</ul>
<h3 class="visible-phone">
  Q: Can I use Superscribe with Asp.Net MVC?
</h3>
<p><strong>A:</strong> Not at present. We'll be looking into whether or not this is possible in the new year so stay tuned.
</p>
<h3 class="visible-phone">
  Q: Can I use Superscribe with Nancy FX?
</h3>
<p><strong>A:</strong> At present you can use Owin routing to hand off from your middleware pipeline to Nancy but then Nancy's routing will take over. Work is under development to allow Nancy to re-use the results of Supersribes route parsing, so watch this space.
</p>
</div>