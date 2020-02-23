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
import theme from 'prism-react-renderer/themes/nightOwl';
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
    ...(theme.plain as any),
  },
  editorArea: {
    overflow: 'auto',
  },
  panelContainerLeftRight: {
    display: 'flex',
    flexDirection: 'row',
    overflow: 'hidden',
    xtouchAction: 'none',
  },
  panelContainerTopBottom: {
    display: 'flex',
    flexDirection: 'column',
    height: '100%',
  },
  panelLeft: {
    width: '45%',
    minHeight: '200px',
    minWidth: '200px',
    whiteSpace: 'nowrap',
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
    whiteSpace: 'nowrap',
    overflow: 'auto',
    height: '50%',
    ...(theme.plain as any),
  },
  splitterTopBottom: {
    cursor: 'row-resize',
  },
  panelBottomLeft: {
    // flex: '1',
  },
});

const evaluateDebounced = _.debounce((code: string, entry: any) => {
  try {
    const r = Babel.transform(
      'const entry = ' +
        JSON.stringify(entry) +
        ';\n' +
        code +
        "\n;\n ReactDOM.render(<Template />, document.getElementById('result'));",
      {
        presets: ['es2015'],
        plugins: ['transform-react-jsx'],
      }
    );
    eval((r as any).code);
  } catch (e) {
    return 'Error';
  }
}, 1000);

export default function App() {
  const [code, setCode] = useState(
    'const Template = () => <h1>Hello EAN <i>{entry.ean}</i></h1>;'
  );
  const [entry, setEntry] = useState("{ ean: '12345678901112' }");
  const classes = useStyles();
  const handleCodeChange = useCallback(
    (code: string) => {
      setCode(code);
      evaluateDebounced(code, entry);
    },
    [code]
  );
  const handleEntryChange = useCallback(
    (entry: string) => {
      setEntry(entry);
      evaluateDebounced(code, JSON.parse(entry));
    },
    [entry]
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
  const highlight = useCallback(
    (code: string) => (
      <Highlight {...defaultProps} code={code} theme={theme} language="jsx">
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

  return (
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
                    highlight={highlight}
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
                Lul
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
            style={{ flexGrow: 1 }}
          >
            <div
              id="result"
              style={{ width: '100%', height: '100%', background: 'white' }}
            />
          </Grid>
        </Grid>
      </main>
      {/* Footer */}
      {/* <footer className={classes.footer}>
        <Typography variant="h6" align="center" gutterBottom>
          Footer
        </Typography>
        <Typography
          variant="subtitle1"
          align="center"
          color="textSecondary"
          component="p"
        >
          Something here to give the footer a purpose!
        </Typography>
        <Copyright />
      </footer> */}
      {/* End footer */}
    </>
  );
}
