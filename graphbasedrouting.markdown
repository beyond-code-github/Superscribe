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
      <li class="active"> <a href="#tab1" data-toggle="tab">Overview<small>What is Graph Based Routing and why is it useful?</small><i class="icon-angle-right"></i></a> </li>
      <li><a href="#tab2" data-toggle="tab">Specification<small>Formal specification and rules shared by all Graph Based Routing implementations</small><i class="icon-angle-right"></i></a> </li>
      <li><a href="#tab3" data-toggle="tab">Pseudocode<small>A language agnostic implmentation for reference purposes</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
    <div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="tab1">
        <h3 class="visible-phone">What is Graph Based Routing?</h3>
        <p>Graph based routing is a <strong>Url parsing and matching strategy</strong> most commonly used as part of a Web Applications. Instead of a routing table containing string definitions, route definitions are stored in a graph structure as <strong>complex objects</strong>. This allows for us to attach methods and functions to our definitions for use during the matching process, and to execute code when a route segment is matched succesfully. This ability to execute arbritary code is known as the <strong>Routing Pipeline</strong></p>
        <p>The advantages of Graph based routing can be summarised as follows:</p>
        <ul>
          <li><strong>Efficiency</strong> - Matches for the next segment are only ever considered from the edges of the current node</li>
          <li><strong>Composibility</strong> - Nodes can be assigned to variables, passed around and reused in multiple definitions</li>
          <li><strong>Extensibility</strong> - Free to create new types of node with any desired behavior</li>
        </ul>
        <p>For example, consider the following routes and below, their graph representation:</p>
        <pre class="prettyprint">
  //  /api/products/{id}
  //  /api/products/bestsellers
  //  /api/customers/{id}
        </pre>
        <img src="img/graph-complex.png" />
        <br/>
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
        <p>The defined route nodes and their Functions form a simple state machine, which can then be used to perform the matching process as seen in this example from Asp.Net Web API:</p>
          <pre class="prettyprint">
  //  Default Web API routing case:
  //    /api/{controller}/{id}
        </pre>
        <img src="img/basicstatemachine.png" />
        <h3 class="visible-phone title">Parameters and the Querystring</h3>
        <p>
          In Graph Based Routing, parameter capture is the responsibility of Action Functions defined against nodes that represent values. They are then added them to the <strong>Parameters</strong> Dictionary which sits on the <strong>RouteData</strong> object so they become available to subsequent nodes and final function. No provision is made in the implementation itself to capture parameters at this time, with the exception of the Querystring.</p>
        <p>
          Because Querystring parameters can come in any order, they cannot be treated as graph nodes for the purposes of matching. Any compatible Graph Based Routing implementation will decode and store all querystring parameters <strong>prior to matching</strong> the remainder of the URL. This means that all nodes have access to querystring values, and if matching based on these values is required this can be performed accordingly in the relevant Activation Function.
        </p>
      </div>
      <div class="tab-pane col-sm-12 col-md-12" id="tab2">
        <h3 class="visible-phone">Graph Based Routing Specification</h3>
      </div>
      <div class="tab-pane col-sm-12 col-md-12" id="tab3">
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

    CALL WalkRouteRecursive WITH routedata, basenode
  END FUNCTION


  FUNCTION WalkRouteRecursive (dictionary routedata, graphnode match)  
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