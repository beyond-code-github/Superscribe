---
layout: default
title:  Features
---

<div class="block">
    <h2 class="title-divider"><span>Graph <span class="de-em">Based Routing</span></span>
    <small>Find out more about the core concepts behind Superscribe</small>
    </h2>
    <div class="tabbable tabs-left vertical-tabs bold-tabs row">
    <ul class="nav nav-tabs nav-stacked col-md-4">
      <li class="active"> <a href="#overview" data-toggle="tab">Overview<small>What is Graph Based Routing and why is it useful?</small><i class="icon-angle-right"></i></a> </li>
      <li><a href="#pseudocode" data-toggle="tab">Pseudocode<small>A language agnostic implmentation for reference purposes</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
    <div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="overview">
        <h3 class="visible-phone">What is Graph Based Routing?</h3>
        <p>Graph based routing is a <strong>Url parsing and matching strategy</strong> most commonly used as part of a Web Applications. Instead of a routing table containing string definitions, route definitions are stored in a graph structure as <strong>complex objects</strong>. This allows for us to attach methods and functions to our definitions for use during the matching process, and to execute code when a route segment is matched succesfully. This ability to execute arbritary code is known as the <strong>Routing Pipeline</strong></p>
        <p>For example, consider the following routes and below, their graph representation:</p>
        <ul>
          <li><em>/api/products/{id}</em></li>
          <li><em>/api/products/bestsellers</em></li>
          <li><em>/api/customers/{id}</em></li>  
        </ul>
        <img src="img/graph-complex.png" />
        <br/>
        <p>The advantages of Graph based routing can be summarised as follows:</p>
        <ul>
          <li><strong>Efficiency</strong> - Matches for the next segment are only ever considered from the edges of the current node</li>
          <li><strong>Composibility</strong> - Nodes can be assigned to variables, passed around and reused in multiple definitions</li>
          <li><strong>Extensibility</strong> - Free to create new types of node with any desired behavior</li>
        </ul>
        <div class="well well-mini pull-center">
          <em>Don't be fooled by the diagram of a Tree... a Tree is just a type of graph where not all nodes are connect by edges.</em>
        </div>
        <h3 class="visible-phone title">The three types of Function</h3>
        <p>
          Flow through the routing pipeline is controlled by three methods that can be assigned to nodes. These can contain any code at all so long as they conform to the required interface, and are known as the Graph Based Routing <strong>Functions</strong>. Between them, these Functions allow the routing pipeline to take actions, determine whether or not a node is a match for the current segment, and even respond to the Http request once matching has finished.</p>
        <p>
          Data can be passed between nodes using an object called the <strong>RouteData</strong>, which is passed as a parameter to each function, along with the current segment. This Data is a read-write dictionary or property bag that can store any information required. The following Functions are available:
        </p>
        <ul>
          <li><strong>Activation Function</strong> - Returns a boolean true or false indicating if this node is a match for the current segment</li>
          <li><strong>Action Function</strong> - Executes some code upon a succesful match, such as parameter capture</li>
          <li><strong>Final Function</strong> - Run when route matching is complete, and allows us to respond to the http request. Final functions can be Normal or Exclusive</li>
        </ul>
        <p>Final Functions are only executed once matching has finished succesfully. If routing finishes on a node that does not define a final function, then Graph Based Routing dictates that we execute the last <em>non-exclusive</em> Final Function that was encountered.</p>
        <h3 class="visible-phone title">The Matching Process</h3>
        <p>Graph Nodes, their constraints, and Functions form a simple state machine, which can then be used to perform the matching process as seen in this example from Asp.Net Web API:</p>
        <ul>
          <li>Default Web API routing case:<p><em>/api/{controller}/{id}</em></p></li>
        </ul>
        <img src="img/basicstatemachine.png" />
        <p>In addition to the behaviors emergent from the presence of the three Functions, there are also a handful of other constraints that are applied to produce this behavior:</p>
        <ul>
          <li><strong>Nodes can be optional</strong> - Matching will only be considered complete if there are no more segments and no more edges, unless at least one of the edges is optional.</li>
          <li><strong>Nodes have allowed methods</strong> - Nodes with allowed http methods set will only be matched if their allow list contains the method of the current request.
          <li><strong>Final Functions have allowed methods</strong> - Same as above, final functions will only be executed if their allowed method list permits it.
        </ul>
        <h3 class="visible-phone title">Parameters and the Querystring</h3>
        <p>
          In Graph Based Routing, parameter capture is the responsibility of Action Functions defined against nodes that represent values. They are then added them to the <strong>Parameters</strong> Dictionary which sits on the <strong>RouteData</strong> object so they become available to subsequent nodes and final function. No provision is made in the implementation itself to capture parameters at this time, with the exception of the Querystring.</p>
        <p>
          Because Querystring parameters can come in any order, they cannot be treated as graph nodes for the purposes of matching. Any compatible Graph Based Routing implementation will decode and store all querystring parameters <strong>prior to matching</strong> the remainder of the URL. This means that all nodes have access to querystring values, and if matching based on these values is required this can be performed accordingly in the relevant Activation Function.
        </p>
        <h3 class="visible-phone title">Incomplete and Extraneous Matches</h3>
        <p>
        Graph Based Routing implementations can set one of two flags to indicate that there was an error while matching. It is then up to the code invoking the implementation to deal with the error as it sees fit. These are:</p> 
        <ul>
          <li>
            <strong>Incomplete Match</strong> - All segments have been consumed but there are still edges present in the current node. This flag will only be set if:
            <ul>
              <li>The node has a Final Function assigned and...</li>
              <li>Has no one optional edges</li>
            </ul>
          </li>
          <li>
            <strong>Extraneous Match</strong> - There are no more edges present in the current node, but we still have route segments remaining to match
          </li>
        </ul>
        <br/>
        <div class="well well-mini pull-center">
          <em>For further reading, please see the original <a href="http://roysvork.wordpress.com/2013/08/20/graph-based-routing/">Graph Based Routing blog post</a></em>
        </div>
      </div>
      <div class="tab-pane col-sm-12 col-md-12" id="pseudocode">
        <h3 class="visible-phone">Graph Based Routing Pseudocode</h3>
        <p>The following pseudocode represents a completely standards compliant graph based routing algortihm that can be used a base for any language specific implementations.</p>
         <pre class="prettyprint">

  queue RemainingSegments;
  string Method;

  FUNCTION WalkRoute (string route, string method, dictionary routedata)  
    FOR EACH querystring parameter IN route
      SET routedata.Parameters dictionary entry for [parameter.name] TO parameter.value

    remove querystring from route
    SET RemainingSegments TO array of route split by '/'
    SET Method TO method parameter value

    CALL WalkRouteLoop WITH routedata, basenode
  END FUNCTION


  FUNCTION WalkRouteLoop (dictionary routedata, graphnode match)  
    finalfunction oncomplete;

    WHILE match IS NOT null    
      IF match has an Action Function THEN CALL Action Function WITH routedata, current segment
      IF there are remaining segments THEN DEQUEUE the current segment FROM RemainingSegments

      IF oncomplete IS set AND oncomplete IS exclusive THEN  SET oncomplete TO null
      IF match has any final functions defined for the current method THEN
        SET oncomplete TO final function for this method

      SET nextmatch to result of CALL FindNextMatch WITH routedata, current segment, match.Edges
      IF nextmatch IS null
        AND current match has no Final Function for this method
        AND current match has edges
        AND current match has no optional edges THEN

          SET IncompleteMatch flag to true
          EXIT FUNCTION
      END IF

      SET match TO nextmatch
    END WHILE

    IF there are still remaining segments THEN SET ExtraneousMatch flag to true
    IF oncomplete IS set CALL oncomplete WITH routedata
  END FUNCTION


  FUNCTION FindNextMatch (routedata, string segment, list edges)    
    FOR EACH potential next match IN edges
      IF the potential match allows the current http method
        AND the potential match Activation Function returns true THEN
          SET match to potential match
      ELSE
          SET match to null
      END IF
    END FOR

    RETURN match
  END FUNCTION

        </pre>
      </div>
    </div>
  </div>
</div>