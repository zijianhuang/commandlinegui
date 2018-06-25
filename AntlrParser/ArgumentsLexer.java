// Generated from Arguments.g4 by ANTLR 4.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ArgumentsLexer extends Lexer {
	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		LiteralText=1, IntRange=2, Slash=3, Hyphen=4, QuotedString=5, Colon=6, 
		Equal=7, NUMBER=8, WS=9;
	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	public static final String[] tokenNames = {
		"<INVALID>",
		"LiteralText", "IntRange", "'/'", "'-'", "QuotedString", "':'", "'='", 
		"NUMBER", "WS"
	};
	public static final String[] ruleNames = {
		"LiteralText", "IntRange", "Slash", "Hyphen", "AnyButAssign", "AnyButSeparatorsAndAssign", 
		"QuotedString", "Colon", "Equal", "NUMBER", "AnyString", "INT", "EXPONENT", 
		"WS"
	};


	public ArgumentsLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "Arguments.g4"; }

	@Override
	public String[] getTokenNames() { return tokenNames; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u0430\ud6d1\u8206\uad2d\u4417\uaef1\u8d80\uaadd\2\13h\b\1\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\3\2\6\2!\n\2\r\2\16\2\"\3\3\3"+
		"\3\3\3\3\3\3\4\3\4\3\5\3\5\3\6\3\6\3\7\3\7\3\b\3\b\3\b\3\b\3\t\3\t\3\n"+
		"\3\n\3\13\5\13:\n\13\3\13\3\13\3\13\7\13?\n\13\f\13\16\13B\13\13\5\13"+
		"D\n\13\3\13\3\13\6\13H\n\13\r\13\16\13I\5\13L\n\13\3\13\5\13O\n\13\3\f"+
		"\3\f\3\f\7\fT\n\f\f\f\16\fW\13\f\3\r\6\rZ\n\r\r\r\16\r[\3\16\3\16\5\16"+
		"`\n\16\3\16\6\16c\n\16\r\16\16\16d\3\17\3\17\2\2\20\3\3\5\4\7\5\t\6\13"+
		"\2\r\2\17\7\21\b\23\t\25\n\27\2\31\2\33\2\35\13\3\2\b\7\2\13\f\17\17\""+
		"\"<<??\t\2\13\f\17\17\"\"//\61\61<<??\3\2$$\4\2GGgg\4\2--//\5\2\13\f\17"+
		"\17\"\"n\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3\2\2\2\2\t\3\2\2\2\2\17\3\2\2\2"+
		"\2\21\3\2\2\2\2\23\3\2\2\2\2\25\3\2\2\2\2\35\3\2\2\2\3 \3\2\2\2\5$\3\2"+
		"\2\2\7(\3\2\2\2\t*\3\2\2\2\13,\3\2\2\2\r.\3\2\2\2\17\60\3\2\2\2\21\64"+
		"\3\2\2\2\23\66\3\2\2\2\259\3\2\2\2\27U\3\2\2\2\31Y\3\2\2\2\33]\3\2\2\2"+
		"\35f\3\2\2\2\37!\5\r\7\2 \37\3\2\2\2!\"\3\2\2\2\" \3\2\2\2\"#\3\2\2\2"+
		"#\4\3\2\2\2$%\5\31\r\2%&\5\t\5\2&\'\5\31\r\2\'\6\3\2\2\2()\7\61\2\2)\b"+
		"\3\2\2\2*+\7/\2\2+\n\3\2\2\2,-\n\2\2\2-\f\3\2\2\2./\n\3\2\2/\16\3\2\2"+
		"\2\60\61\7$\2\2\61\62\5\27\f\2\62\63\7$\2\2\63\20\3\2\2\2\64\65\7<\2\2"+
		"\65\22\3\2\2\2\66\67\7?\2\2\67\24\3\2\2\28:\7/\2\298\3\2\2\29:\3\2\2\2"+
		":C\3\2\2\2;D\7\62\2\2<@\4\63;\2=?\5\31\r\2>=\3\2\2\2?B\3\2\2\2@>\3\2\2"+
		"\2@A\3\2\2\2AD\3\2\2\2B@\3\2\2\2C;\3\2\2\2C<\3\2\2\2DK\3\2\2\2EG\7\60"+
		"\2\2FH\5\31\r\2GF\3\2\2\2HI\3\2\2\2IG\3\2\2\2IJ\3\2\2\2JL\3\2\2\2KE\3"+
		"\2\2\2KL\3\2\2\2LN\3\2\2\2MO\5\33\16\2NM\3\2\2\2NO\3\2\2\2O\26\3\2\2\2"+
		"PQ\7$\2\2QT\7$\2\2RT\n\4\2\2SP\3\2\2\2SR\3\2\2\2TW\3\2\2\2US\3\2\2\2U"+
		"V\3\2\2\2V\30\3\2\2\2WU\3\2\2\2XZ\4\62;\2YX\3\2\2\2Z[\3\2\2\2[Y\3\2\2"+
		"\2[\\\3\2\2\2\\\32\3\2\2\2]_\t\5\2\2^`\t\6\2\2_^\3\2\2\2_`\3\2\2\2`b\3"+
		"\2\2\2ac\4\62;\2ba\3\2\2\2cd\3\2\2\2db\3\2\2\2de\3\2\2\2e\34\3\2\2\2f"+
		"g\t\7\2\2g\36\3\2\2\2\17\2\"9@CIKNSU[_d\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}