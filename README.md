#Simple
Simple is a toy homemade scripting language implemented in C#. Started as a Simple IMPerative LanguageE implementation for a university course in Models of Computation, it has been extended with closures. Of course, it has an educational purpose only. It is not meant to be efficient nor complete. The project is closed, since it will be the basis for a more efficient Simple implementation (in C++).

Simple's (LL(2), recursive-descent parsed) grammar:

<Axiom> 		  :: <Program> Terminator
<Program> 		:: <Expression>
				       | eps
<Expression> 	:: if <Expression> then <Expression>
			  	     | if <Expression> then <Expression> else <Expression>
			  	     | loop <Expression> while <Expression>
		 	  	     | while <Expression> loop <Expression>
		 	  	     | do <Expression>
		 	  	     | <Value> <Continuation>
<Continuation>:: ( <Sequence> )
		 	  	     | = <Expression>
		 	  	     | eps
<Value>			  :: number
				       | identifier
				       | undefined
				       | function ( <Params> ) <Expression>
				       | [ <Expression> ]
				       | <Block>
<Sequence>		:: <Expression> <Sequence>
				       | eps
<Names>			  :: identifier <Names>
				       | eps
<Block>			  :: { <Sequence> }
			 
In Simple, every syntactical construct is an expression.
An expression returns a list of values, which can be numbers and lists too.
- A number represents a list with only one element, and viceversa
- An empty list has undefined value. A list never contains undefined values inside
- You can build lists with more than one value using blocks 
- You can prevent a value to be returned using the "do" keyword

Some examples:

	i = undefined				# equivalent to {}, {undefined}, {{undefined} {undefined}}, etc
	i = 0 							# equivalent to {0}, {{0}}, {{{0}}}, etc
	i = {1 2 3}					# equivalent to {1 2 3}, {{1 2 3}}, {1 2 3 undefined}, etc
	
	   a = if 1 then {1} else {2}					            # returns 1
	do a = if 1 then {1} else {2}					            # returns undefined
	   a = while neq(n 0) loop {n do n = sub(n 1)}	  # returns {1 .. n}
	
Some recursion and functional programming examples:	
	
	plus  = function(x y) sum(x y)
	ssum  = function(n) if eq(n 0) then 0 else sum(n ssum(sub(n 1)))
	fact  = function(n) if eq(n 0) then 1 else mul(n fact(sub(n 1)))
	fib   = function(n) if eq(n 0) then 0 else if eq(n 1) then 1 else sum(fib(sub(n 2)) fib(sub(n 1)))
	fix   = function(f) f(fix(f))
	curry = function(f) function(x) function(y) f(x y)
	apply = function(f x) f(x)
	range = function(n) while neq(n 0) loop { n do n = sub(n 1) }
	
