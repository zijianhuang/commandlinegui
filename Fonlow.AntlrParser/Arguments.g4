grammar Arguments;
init : fixedParameter? (WS+ fixedParameter)* WS* parameters WS*;

parameters : parameter (WS+ parameter)* ;
parameter : (Slash | Hyphen) parameterName 
            | (Slash | Hyphen) parameterName (Colon | Equal | WS+) (value WS*)+ ;

parameterName : LiteralText | LiteralText Hyphen ;

fixedParameter : quotedString | LiteralText | value;

value :   quotedString 
      | LiteralText ((Slash | Hyphen | Equal | Colon)+ LiteralText*)+ 
      | LiteralText Equal LiteralText
      | IntRange
      | NUMBER
      | LiteralText
      ;

LiteralText : AnyButSeparatorsAndAssign+ ;

IntRange : INT Hyphen INT;

Slash : '/' ;

Hyphen : '-';

fragment AnyButAssign : ~(
        
        [ \t\r\n]  //WS skip
        | ':' | '='// Assign
        | ' '
        );

fragment AnyButSeparatorsAndAssign : ~(
         '/' | '-'  //Separator
        | [ \t\r\n]  //WS skip
        | ':' | '='// Assign
        | ' '
        );

quotedString : QuotedString;
QuotedString : '"' AnyString '"' ; // quote-quote is an escaped quote

Colon : ':';

Equal : '=';

NUMBER : '-'? ('0' | ( '1'..'9' INT* )) ('.' INT+)? EXPONENT?;


fragment AnyString : ('""'|~'"')*;

fragment INT : '0'..'9'+;

fragment EXPONENT : ('e'|'E') ('+'|'-')? ('0'..'9')+;
            
WS  :   [ \t\r\n];// Do not skip. Space could have meaning in command line   

