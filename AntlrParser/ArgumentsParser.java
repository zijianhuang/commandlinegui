// Generated from Arguments.g4 by ANTLR 4.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ArgumentsParser extends Parser {
	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		LiteralText=1, IntRange=2, Slash=3, Hyphen=4, QuotedString=5, Colon=6, 
		Equal=7, NUMBER=8, WS=9;
	public static final String[] tokenNames = {
		"<INVALID>", "LiteralText", "IntRange", "'/'", "'-'", "QuotedString", 
		"':'", "'='", "NUMBER", "WS"
	};
	public static final int
		RULE_init = 0, RULE_parameters = 1, RULE_parameter = 2, RULE_parameterName = 3, 
		RULE_fixedParameter = 4, RULE_value = 5, RULE_quotedString = 6;
	public static final String[] ruleNames = {
		"init", "parameters", "parameter", "parameterName", "fixedParameter", 
		"value", "quotedString"
	};

	@Override
	public String getGrammarFileName() { return "Arguments.g4"; }

	@Override
	public String[] getTokenNames() { return tokenNames; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public ArgumentsParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}
	public static class InitContext extends ParserRuleContext {
		public List<FixedParameterContext> fixedParameter() {
			return getRuleContexts(FixedParameterContext.class);
		}
		public TerminalNode WS(int i) {
			return getToken(ArgumentsParser.WS, i);
		}
		public FixedParameterContext fixedParameter(int i) {
			return getRuleContext(FixedParameterContext.class,i);
		}
		public List<TerminalNode> WS() { return getTokens(ArgumentsParser.WS); }
		public ParametersContext parameters() {
			return getRuleContext(ParametersContext.class,0);
		}
		public InitContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_init; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterInit(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitInit(this);
		}
	}

	public final InitContext init() throws RecognitionException {
		InitContext _localctx = new InitContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_init);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(15);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << LiteralText) | (1L << IntRange) | (1L << QuotedString) | (1L << NUMBER))) != 0)) {
				{
				setState(14); fixedParameter();
				}
			}

			setState(25);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			while ( _alt!=2 && _alt!=-1 ) {
				if ( _alt==1 ) {
					{
					{
					setState(18); 
					_errHandler.sync(this);
					_la = _input.LA(1);
					do {
						{
						{
						setState(17); match(WS);
						}
						}
						setState(20); 
						_errHandler.sync(this);
						_la = _input.LA(1);
					} while ( _la==WS );
					setState(22); fixedParameter();
					}
					} 
				}
				setState(27);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			}
			setState(31);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==WS) {
				{
				{
				setState(28); match(WS);
				}
				}
				setState(33);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(34); parameters();
			setState(38);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==WS) {
				{
				{
				setState(35); match(WS);
				}
				}
				setState(40);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParametersContext extends ParserRuleContext {
		public List<ParameterContext> parameter() {
			return getRuleContexts(ParameterContext.class);
		}
		public TerminalNode WS(int i) {
			return getToken(ArgumentsParser.WS, i);
		}
		public List<TerminalNode> WS() { return getTokens(ArgumentsParser.WS); }
		public ParameterContext parameter(int i) {
			return getRuleContext(ParameterContext.class,i);
		}
		public ParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameters; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterParameters(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitParameters(this);
		}
	}

	public final ParametersContext parameters() throws RecognitionException {
		ParametersContext _localctx = new ParametersContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_parameters);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(41); parameter();
			setState(50);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			while ( _alt!=2 && _alt!=-1 ) {
				if ( _alt==1 ) {
					{
					{
					setState(43); 
					_errHandler.sync(this);
					_la = _input.LA(1);
					do {
						{
						{
						setState(42); match(WS);
						}
						}
						setState(45); 
						_errHandler.sync(this);
						_la = _input.LA(1);
					} while ( _la==WS );
					setState(47); parameter();
					}
					} 
				}
				setState(52);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParameterContext extends ParserRuleContext {
		public List<ValueContext> value() {
			return getRuleContexts(ValueContext.class);
		}
		public TerminalNode WS(int i) {
			return getToken(ArgumentsParser.WS, i);
		}
		public ValueContext value(int i) {
			return getRuleContext(ValueContext.class,i);
		}
		public List<TerminalNode> WS() { return getTokens(ArgumentsParser.WS); }
		public ParameterNameContext parameterName() {
			return getRuleContext(ParameterNameContext.class,0);
		}
		public TerminalNode Colon() { return getToken(ArgumentsParser.Colon, 0); }
		public TerminalNode Equal() { return getToken(ArgumentsParser.Equal, 0); }
		public TerminalNode Slash() { return getToken(ArgumentsParser.Slash, 0); }
		public TerminalNode Hyphen() { return getToken(ArgumentsParser.Hyphen, 0); }
		public ParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitParameter(this);
		}
	}

	public final ParameterContext parameter() throws RecognitionException {
		ParameterContext _localctx = new ParameterContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_parameter);
		int _la;
		try {
			int _alt;
			setState(77);
			switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(53);
				_la = _input.LA(1);
				if ( !(_la==Slash || _la==Hyphen) ) {
				_errHandler.recoverInline(this);
				}
				consume();
				setState(54); parameterName();
				}
				break;

			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(55);
				_la = _input.LA(1);
				if ( !(_la==Slash || _la==Hyphen) ) {
				_errHandler.recoverInline(this);
				}
				consume();
				setState(56); parameterName();
				setState(64);
				switch (_input.LA(1)) {
				case Colon:
					{
					setState(57); match(Colon);
					}
					break;
				case Equal:
					{
					setState(58); match(Equal);
					}
					break;
				case WS:
					{
					setState(60); 
					_errHandler.sync(this);
					_la = _input.LA(1);
					do {
						{
						{
						setState(59); match(WS);
						}
						}
						setState(62); 
						_errHandler.sync(this);
						_la = _input.LA(1);
					} while ( _la==WS );
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(73); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(66); value();
					setState(70);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,9,_ctx);
					while ( _alt!=2 && _alt!=-1 ) {
						if ( _alt==1 ) {
							{
							{
							setState(67); match(WS);
							}
							} 
						}
						setState(72);
						_errHandler.sync(this);
						_alt = getInterpreter().adaptivePredict(_input,9,_ctx);
					}
					}
					}
					setState(75); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << LiteralText) | (1L << IntRange) | (1L << QuotedString) | (1L << NUMBER))) != 0) );
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParameterNameContext extends ParserRuleContext {
		public TerminalNode LiteralText() { return getToken(ArgumentsParser.LiteralText, 0); }
		public TerminalNode Hyphen() { return getToken(ArgumentsParser.Hyphen, 0); }
		public ParameterNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameterName; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterParameterName(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitParameterName(this);
		}
	}

	public final ParameterNameContext parameterName() throws RecognitionException {
		ParameterNameContext _localctx = new ParameterNameContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_parameterName);
		try {
			setState(82);
			switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(79); match(LiteralText);
				}
				break;

			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(80); match(LiteralText);
				setState(81); match(Hyphen);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FixedParameterContext extends ParserRuleContext {
		public QuotedStringContext quotedString() {
			return getRuleContext(QuotedStringContext.class,0);
		}
		public ValueContext value() {
			return getRuleContext(ValueContext.class,0);
		}
		public TerminalNode LiteralText() { return getToken(ArgumentsParser.LiteralText, 0); }
		public FixedParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fixedParameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterFixedParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitFixedParameter(this);
		}
	}

	public final FixedParameterContext fixedParameter() throws RecognitionException {
		FixedParameterContext _localctx = new FixedParameterContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_fixedParameter);
		try {
			setState(87);
			switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(84); quotedString();
				}
				break;

			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(85); match(LiteralText);
				}
				break;

			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(86); value();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ValueContext extends ParserRuleContext {
		public List<TerminalNode> Colon() { return getTokens(ArgumentsParser.Colon); }
		public TerminalNode Hyphen(int i) {
			return getToken(ArgumentsParser.Hyphen, i);
		}
		public List<TerminalNode> Slash() { return getTokens(ArgumentsParser.Slash); }
		public TerminalNode Colon(int i) {
			return getToken(ArgumentsParser.Colon, i);
		}
		public TerminalNode Slash(int i) {
			return getToken(ArgumentsParser.Slash, i);
		}
		public List<TerminalNode> Hyphen() { return getTokens(ArgumentsParser.Hyphen); }
		public TerminalNode LiteralText(int i) {
			return getToken(ArgumentsParser.LiteralText, i);
		}
		public QuotedStringContext quotedString() {
			return getRuleContext(QuotedStringContext.class,0);
		}
		public TerminalNode IntRange() { return getToken(ArgumentsParser.IntRange, 0); }
		public List<TerminalNode> LiteralText() { return getTokens(ArgumentsParser.LiteralText); }
		public List<TerminalNode> Equal() { return getTokens(ArgumentsParser.Equal); }
		public TerminalNode NUMBER() { return getToken(ArgumentsParser.NUMBER, 0); }
		public TerminalNode Equal(int i) {
			return getToken(ArgumentsParser.Equal, i);
		}
		public ValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterValue(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitValue(this);
		}
	}

	public final ValueContext value() throws RecognitionException {
		ValueContext _localctx = new ValueContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_value);
		int _la;
		try {
			int _alt;
			setState(112);
			switch ( getInterpreter().adaptivePredict(_input,17,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(89); quotedString();
				}
				break;

			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(90); match(LiteralText);
				setState(102); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(92); 
						_errHandler.sync(this);
						_alt = getInterpreter().adaptivePredict(_input,14,_ctx);
						do {
							switch (_alt) {
							case 1:
								{
								{
								setState(91);
								_la = _input.LA(1);
								if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << Slash) | (1L << Hyphen) | (1L << Colon) | (1L << Equal))) != 0)) ) {
								_errHandler.recoverInline(this);
								}
								consume();
								}
								}
								break;
							default:
								throw new NoViableAltException(this);
							}
							setState(94); 
							_errHandler.sync(this);
							_alt = getInterpreter().adaptivePredict(_input,14,_ctx);
						} while ( _alt!=2 && _alt!=-1 );
						setState(99);
						_errHandler.sync(this);
						_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
						while ( _alt!=2 && _alt!=-1 ) {
							if ( _alt==1 ) {
								{
								{
								setState(96); match(LiteralText);
								}
								} 
							}
							setState(101);
							_errHandler.sync(this);
							_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
						}
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(104); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
				} while ( _alt!=2 && _alt!=-1 );
				}
				break;

			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(106); match(LiteralText);
				setState(107); match(Equal);
				setState(108); match(LiteralText);
				}
				break;

			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(109); match(IntRange);
				}
				break;

			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(110); match(NUMBER);
				}
				break;

			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(111); match(LiteralText);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class QuotedStringContext extends ParserRuleContext {
		public TerminalNode QuotedString() { return getToken(ArgumentsParser.QuotedString, 0); }
		public QuotedStringContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_quotedString; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).enterQuotedString(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ArgumentsListener ) ((ArgumentsListener)listener).exitQuotedString(this);
		}
	}

	public final QuotedStringContext quotedString() throws RecognitionException {
		QuotedStringContext _localctx = new QuotedStringContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_quotedString);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(114); match(QuotedString);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\3\u0430\ud6d1\u8206\uad2d\u4417\uaef1\u8d80\uaadd\3\13w\4\2\t\2\4\3\t"+
		"\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\3\2\5\2\22\n\2\3\2\6\2\25\n"+
		"\2\r\2\16\2\26\3\2\7\2\32\n\2\f\2\16\2\35\13\2\3\2\7\2 \n\2\f\2\16\2#"+
		"\13\2\3\2\3\2\7\2\'\n\2\f\2\16\2*\13\2\3\3\3\3\6\3.\n\3\r\3\16\3/\3\3"+
		"\7\3\63\n\3\f\3\16\3\66\13\3\3\4\3\4\3\4\3\4\3\4\3\4\3\4\6\4?\n\4\r\4"+
		"\16\4@\5\4C\n\4\3\4\3\4\7\4G\n\4\f\4\16\4J\13\4\6\4L\n\4\r\4\16\4M\5\4"+
		"P\n\4\3\5\3\5\3\5\5\5U\n\5\3\6\3\6\3\6\5\6Z\n\6\3\7\3\7\3\7\6\7_\n\7\r"+
		"\7\16\7`\3\7\7\7d\n\7\f\7\16\7g\13\7\6\7i\n\7\r\7\16\7j\3\7\3\7\3\7\3"+
		"\7\3\7\3\7\5\7s\n\7\3\b\3\b\3\b\2\2\t\2\4\6\b\n\f\16\2\4\3\2\5\6\4\2\5"+
		"\6\b\t\u0087\2\21\3\2\2\2\4+\3\2\2\2\6O\3\2\2\2\bT\3\2\2\2\nY\3\2\2\2"+
		"\fr\3\2\2\2\16t\3\2\2\2\20\22\5\n\6\2\21\20\3\2\2\2\21\22\3\2\2\2\22\33"+
		"\3\2\2\2\23\25\7\13\2\2\24\23\3\2\2\2\25\26\3\2\2\2\26\24\3\2\2\2\26\27"+
		"\3\2\2\2\27\30\3\2\2\2\30\32\5\n\6\2\31\24\3\2\2\2\32\35\3\2\2\2\33\31"+
		"\3\2\2\2\33\34\3\2\2\2\34!\3\2\2\2\35\33\3\2\2\2\36 \7\13\2\2\37\36\3"+
		"\2\2\2 #\3\2\2\2!\37\3\2\2\2!\"\3\2\2\2\"$\3\2\2\2#!\3\2\2\2$(\5\4\3\2"+
		"%\'\7\13\2\2&%\3\2\2\2\'*\3\2\2\2(&\3\2\2\2()\3\2\2\2)\3\3\2\2\2*(\3\2"+
		"\2\2+\64\5\6\4\2,.\7\13\2\2-,\3\2\2\2./\3\2\2\2/-\3\2\2\2/\60\3\2\2\2"+
		"\60\61\3\2\2\2\61\63\5\6\4\2\62-\3\2\2\2\63\66\3\2\2\2\64\62\3\2\2\2\64"+
		"\65\3\2\2\2\65\5\3\2\2\2\66\64\3\2\2\2\678\t\2\2\28P\5\b\5\29:\t\2\2\2"+
		":B\5\b\5\2;C\7\b\2\2<C\7\t\2\2=?\7\13\2\2>=\3\2\2\2?@\3\2\2\2@>\3\2\2"+
		"\2@A\3\2\2\2AC\3\2\2\2B;\3\2\2\2B<\3\2\2\2B>\3\2\2\2CK\3\2\2\2DH\5\f\7"+
		"\2EG\7\13\2\2FE\3\2\2\2GJ\3\2\2\2HF\3\2\2\2HI\3\2\2\2IL\3\2\2\2JH\3\2"+
		"\2\2KD\3\2\2\2LM\3\2\2\2MK\3\2\2\2MN\3\2\2\2NP\3\2\2\2O\67\3\2\2\2O9\3"+
		"\2\2\2P\7\3\2\2\2QU\7\3\2\2RS\7\3\2\2SU\7\6\2\2TQ\3\2\2\2TR\3\2\2\2U\t"+
		"\3\2\2\2VZ\5\16\b\2WZ\7\3\2\2XZ\5\f\7\2YV\3\2\2\2YW\3\2\2\2YX\3\2\2\2"+
		"Z\13\3\2\2\2[s\5\16\b\2\\h\7\3\2\2]_\t\3\2\2^]\3\2\2\2_`\3\2\2\2`^\3\2"+
		"\2\2`a\3\2\2\2ae\3\2\2\2bd\7\3\2\2cb\3\2\2\2dg\3\2\2\2ec\3\2\2\2ef\3\2"+
		"\2\2fi\3\2\2\2ge\3\2\2\2h^\3\2\2\2ij\3\2\2\2jh\3\2\2\2jk\3\2\2\2ks\3\2"+
		"\2\2lm\7\3\2\2mn\7\t\2\2ns\7\3\2\2os\7\4\2\2ps\7\n\2\2qs\7\3\2\2r[\3\2"+
		"\2\2r\\\3\2\2\2rl\3\2\2\2ro\3\2\2\2rp\3\2\2\2rq\3\2\2\2s\r\3\2\2\2tu\7"+
		"\7\2\2u\17\3\2\2\2\24\21\26\33!(/\64@BHMOTY`ejr";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}