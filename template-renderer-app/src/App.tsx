/// <reference path="../node_modules/jquery-resizable-dom/src/jquery-resizable.d.ts" />
import React, {
  useState,
  useEffect,
  useCallback,
  useMemo,
  ReactDOM,
} from 'react';
import AppBar from '@material-ui/core/AppBar';
import CssBaseline from '@material-ui/core/CssBaseline';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Editor from 'react-simple-code-editor';
import Highlight, { defaultProps } from 'prism-react-renderer';
import nightOwl from 'prism-react-renderer/themes/nightOwl';
import nightOwlLight from 'prism-react-renderer/themes/nightOwl';
import { makeStyles, Grid } from '@material-ui/core';
import $ from 'jquery';
import 'jquery-resizable-dom';
import _ from 'lodash';
import * as Babel from '@babel/standalone';
import '../src/styles.css';

const useStyles = makeStyles({
  container: {
    position: 'absolute',
    top: '64px',
    bottom: 0,
    left: 0,
    right: 0,
    display: 'flex',
  },
  editorContainer: {
    fontFamily: '"Dank Mono", "Fira Code", monospace',
    maxHeight: '100%',
  },
  editorArea: {
    overflow: 'auto',
  },
  panelContainerLeftRight: {
    display: 'flex',
    flexDirection: 'row',
    overflow: 'hidden',
    xtouchAction: 'none',
    height: '100%',
  },
  panelContainerTopBottom: {
    display: 'flex',
    flexDirection: 'column',
    height: '100%',
  },
  panelLeft: {
    width: '33%',
    minHeight: '200px',
    minWidth: '200px',
    height: '100%',
  },
  splitterLeftRight: {
    cursor: 'col-resize',
  },
  panelRight: {
    height: '100%',
  },
  panelTopLeft: {
    width: '100%',
    overflow: 'auto',
    height: '50%',
    maxHeight: 'calc(100% - 10px)',
    ...(nightOwl.plain as any),
  },
  splitterTopBottom: {
    cursor: 'row-resize',
  },
  panelBottomLeft: {
    overflow: 'auto',
    flex: '1',
    ...(nightOwlLight.plain as any),
  },
});

const evaluateDebounced = _.debounce((code: string, entry: any) => {
  try {
    const r = Babel.transform(
      'const entry = JSON.parse(' +
        JSON.stringify(entry) +
        ');\n' +
        code +
        `;\nclass ErrorBoundary extends React.Component {
          constructor(props) {
            super(props);
            this.state = { hasError: false };
          }
        
          static getDerivedStateFromError = error => ({ hasError: true });
          componentDidCatch = (error, errorInfo) => {};
        
          render() {
            if (this.state.hasError) {
              return <h1>Error</h1>;
            }
            return this.props.children;
          }
        }` +
        "\n;\n ReactDOM.render(<ErrorBoundary><Template /></ErrorBoundary>, document.getElementById('result'));",
      {
        presets: ['es2015'],
        plugins: ['transform-react-jsx', 'proposal-class-properties'],
      }
    );
    console.log((r as any).code);
    eval((r as any).code);
  } catch (e) {
    return 'Error';
  }
}, 500);

export default function App() {
  const [code, setCode] = useState(
    'const Template = () => <h1>Hello EAN <i>{entry.ean}</i></h1>;'
  );
  const [entry, setEntry] = useState('{ "ean": "12345678901112" }');
  const classes = useStyles();
  const handleCodeChange = useCallback(
    (code: string) => {
      setCode(code);
      evaluateDebounced(code, entry);
    },
    [code, entry]
  );
  const handleEntryChange = useCallback(
    (entry: string) => {
      setEntry(entry);
      try {
        evaluateDebounced(
          code,
          entry.replace(/[\\"']/g, '$&').replace(/\u0000/g, '\\0')
        );
      } catch (e) {}
    },
    [code, entry]
  );
  useEffect(() => {
    $('.panel-left').resizable({
      handleSelector: '.splitter-left-right',
      resizeHeight: false,
    });
    $('.panel-top-left').resizable({
      handleSelector: '.splitter-top-bottom',
      resizeWidth: false,
    });
  });
  const highlightCode = useCallback(
    (code: string) => (
      <Highlight {...defaultProps} code={code} theme={nightOwl} language="jsx">
        {({ className, style, tokens, getLineProps, getTokenProps }) => (
          <>
            {tokens.map((line, i) => (
              <div {...getLineProps({ line, key: i })}>
                {line.map((token, key) => (
                  <span {...getTokenProps({ token, key })} />
                ))}
              </div>
            ))}
          </>
        )}
      </Highlight>
    ),
    [code]
  );
  const highlightEntry = useCallback(
    (code: string) => (
      <Highlight
        {...defaultProps}
        code={code}
        theme={nightOwlLight}
        language="json"
      >
        {({ className, style, tokens, getLineProps, getTokenProps }) => (
          <>
            {tokens.map((line, i) => (
              <div {...getLineProps({ line, key: i })}>
                {line.map((token, key) => (
                  <span {...getTokenProps({ token, key })} />
                ))}
              </div>
            ))}
          </>
        )}
      </Highlight>
    ),
    [entry]
  );

  const result = useMemo(
    () => (
      <>
        <CssBaseline />
        <AppBar position="relative">
          <Toolbar>
            {/* <CameraIcon className={classes.icon} /> */}
            <Typography variant="h6" color="inherit" noWrap>
              TemplateRenderer
            </Typography>
          </Toolbar>
        </AppBar>
        <main className={classes.container}>
          <Grid
            container
            direction="row"
            className={classes.panelContainerLeftRight}
          >
            <Grid item className={classes.panelLeft + ' panel-left'}>
              <Grid
                container
                direction="column"
                className={classes.panelContainerTopBottom}
              >
                <Grid item className={classes.panelTopLeft + ' panel-top-left'}>
                  <div className={classes.editorArea}>
                    <Editor
                      value={code}
                      onValueChange={handleCodeChange}
                      highlight={highlightCode}
                      className={classes.editorContainer}
                      padding={10}
                    />
                  </div>
                </Grid>
                <Grid
                  item
                  className={classes.splitterTopBottom + ' splitter-top-bottom'}
                  style={{ height: '10px', background: 'silver' }}
                />
                <Grid
                  item
                  className={classes.panelBottomLeft + ' panel-bottom-left'}
                >
                  <div className={classes.editorArea}>
                    <Editor
                      value={entry}
                      onValueChange={handleEntryChange}
                      highlight={highlightEntry}
                      className={classes.editorContainer}
                      padding={10}
                    />
                  </div>
                </Grid>
              </Grid>
            </Grid>
            <Grid
              item
              className={classes.splitterLeftRight + ' splitter-left-right'}
              style={{ width: '10px', background: 'silver' }}
            />
            <Grid
              className={classes.panelRight + ' panel-right'}
              style={{ flexGrow: 1, background: 'white' }}
            >
              <div
                id="result"
                style={{
                  width: '100%',
                  height: '100%',
                }}
              />
            </Grid>
          </Grid>
        </main>
      </>
    ),
    [code, setCode, entry, setEntry]
  );
  return <>{result}</>;
}
